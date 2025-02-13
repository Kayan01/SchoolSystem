﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.PubSub
{
    public interface IBackgoundPublishService
    {
        Task RetryPublish();
    }
    public class BackgroundPublishService : IBackgoundPublishService
    {
        private readonly IProducerClient<BusMessage> _producerClient;
        private IRepository<PublishedMessage, Guid> _pubMessageRepository;
        private IUnitOfWork _unitOfWork;
        private ILogger<BackgroundPublishService> _logger;
        private static readonly object _object = new object();

        public BackgroundPublishService(IServiceScopeFactory serviceScopeFactory)
        {
            var scope = serviceScopeFactory.CreateScope();
            _producerClient = scope.ServiceProvider.GetRequiredService<IProducerClient<BusMessage>>();
            _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            _pubMessageRepository = scope.ServiceProvider.GetRequiredService<IRepository<PublishedMessage, Guid>>();
            _logger = scope.ServiceProvider.GetRequiredService<ILogger<BackgroundPublishService>>();
        }

        public async Task RetryPublish()
        {
            _logger.Log(LogLevel.Information, "Running Background retry job");
            //Reset Message Stuck on Sending
            await ResetMessageStuckInSending();
            //Get all Messages to be sent
            var pubMessages = _pubMessageRepository.GetAll().AsNoTracking().Where(x => x.Status == Enums.MessageStatus.Pending).AsNoTracking().Take(100).ToList();
            if (pubMessages.Any())
            {
                foreach (var pubMessage in pubMessages)
                {
                    //Try to publish
                    var pubMessageResult = TryPublishMessage(pubMessage);
                    if (pubMessageResult.Status != Enums.MessageStatus.Completed)
                        break;
                }
            }
            //wait for message that are attempting publishing. Status == Sending
            Console.WriteLine($"Delaying for 3 mins. . . ");
            Thread.Sleep(60000 * 3);// delay for 3 mins 60000 * 3
            pubMessages = _pubMessageRepository.GetAll().AsNoTracking().Where(x => x.Status == Enums.MessageStatus.Sending).AsNoTracking().Take(100).ToList();


            //If there is any message in sending skip 30 mins delay 
            if (!pubMessages.Any())
            {
                Console.WriteLine($"Delaying for 30 mins. . . ");
                Thread.Sleep(60000 * 30);// delay for 30 mins 60000 * 30
            }
        }

        private async Task ResetMessageStuckInSending()
        {
            Console.WriteLine($"Reseting ...");
            //Reset all message stuck in sending for over 5 mins
            var fiveMinsAgo = DateTime.Now.AddMinutes(-5);
            var stuckMessages = _pubMessageRepository.GetAll().Where(x => x.Status == Enums.MessageStatus.Sending && x.LastModificationTime <= fiveMinsAgo ||
                x.Status == Enums.MessageStatus.Sending && x.CreationTime <= fiveMinsAgo && x.LastModificationTime == null).ToList();

            stuckMessages.ForEach(x => x.Status = Enums.MessageStatus.Pending);
            Console.WriteLine($"Reseting ...{stuckMessages.Count()}");
            await _unitOfWork.SaveChangesAsync();
        }

        private PublishedMessage TryPublishMessage(PublishedMessage pubMessage)
        {
            Console.WriteLine($"Attempting To Publishing...");
            var busMessage = new BusMessage
            {
                Data = pubMessage.Message,
                BusMessageType = (int)pubMessage.MessageType,
            };

            lock (_object)
            {
                pubMessage = _pubMessageRepository.FirstOrDefault(x => x.Id == pubMessage.Id);

                if (pubMessage.Status != Enums.MessageStatus.Pending)
                    return pubMessage;

                pubMessage.Status = Enums.MessageStatus.Sending;
                _pubMessageRepository.Update(pubMessage);
                _unitOfWork.SaveChanges();
            }

            try
            {
                var deliveryResult = _producerClient.Produce(pubMessage.Topic, busMessage).Result;
                if (deliveryResult.Status != Confluent.Kafka.PersistenceStatus.NotPersisted)
                {
                    pubMessage.Status = Enums.MessageStatus.Completed;
                    _pubMessageRepository.Update(pubMessage);
                    _unitOfWork.SaveChanges();
                }
                else
                {
                    pubMessage.Status = Enums.MessageStatus.Pending;
                    _pubMessageRepository.Update(pubMessage);
                    _unitOfWork.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to broadcast Message: {ex.Message}");
                pubMessage.Status = Enums.MessageStatus.Pending;
                _unitOfWork.SaveChanges();
            }

            return pubMessage;
        }
    }
}
