using Microsoft.EntityFrameworkCore;
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
    public interface IPublishService
    {
        Task PublishMessage(string topic, BusMessageTypes messageType, object data);
    }

    public class PublishService : IPublishService
    {
        private readonly IProducerClient<BusMessage> _producerClient;
        private readonly IRepository<PublishedMessage, Guid> _pubMessageRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PublishService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private bool retryServiceIsRunning;

        public PublishService(IServiceScopeFactory serviceScopeFactory,
            IProducerClient<BusMessage> producerClient,
            IRepository<PublishedMessage, Guid> pubMessageRepository,
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _pubMessageRepository = pubMessageRepository;
            _producerClient = producerClient;
            //using var scope = serviceScopeFactory.CreateScope();
            //_unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            //_pubMessageRepository = scope.ServiceProvider.GetRequiredService<IRepository<PublishedMessage, Guid>>();
        }


        public async Task PublishMessage(string topic, BusMessageTypes messageType, object data)
        {
            var pubMessage = _pubMessageRepository.Insert(new PublishedMessage
            {
                Topic = topic,
                Message = JsonConvert.SerializeObject(data),
                MessageType = messageType,
                Status = Enums.MessageStatus.Sending,
            });
            _unitOfWork.SaveChanges();

            Task.Run(() => Publish(pubMessage)); 
            //var BGPublish = new BackgroundPublishService(_serviceScopeFactory, _producerClient);
            //Task.Run(() => BGPublish.Publish(pubMessage));
        }

        private void Publish(PublishedMessage message)
        {
            var pubMessage = _pubMessageRepository.FirstOrDefault(x => x.Id == message.Id);

            try
            {
                var deliveryResult = _producerClient.Produce(message.Topic, 
                        new BusMessage((int)message.MessageType,message.Message)).Result;

                if (deliveryResult.Status != Confluent.Kafka.PersistenceStatus.NotPersisted)
                    pubMessage.Status = Enums.MessageStatus.Completed;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to broadcast Message: {ex.Message}");
                pubMessage.Status = Enums.MessageStatus.Pending;
            }
            finally
            {
                _unitOfWork.SaveChanges();

                if (!retryServiceIsRunning)
                    RetryPendingMessages();//begin retry protocol
            }
        }

        private void RetryPendingMessages()
        {
            var keepRunningFlag = true;
            while (keepRunningFlag)
            {
                keepRunningFlag = false;
                //TODO using Blocking COllection
                var pubServiceIsActive = true; //true until otherwise

                ResetMessageStuckInSending();

                var pubMessages = _pubMessageRepository.GetAll().AsNoTracking().Where(x => x.Status == Enums.MessageStatus.Pending).AsNoTracking().Take(100).ToList();

                if (pubMessages.Count > 100)//100 at a time to reduce number of items in memory
                    keepRunningFlag = true;

                if (pubMessages.Any())
                {
                    pubServiceIsActive = false;
                    long delay = 0;
                    while (!pubServiceIsActive)
                    {
                        Console.WriteLine($"Delaying for {delay}. . . ");
                        Thread.Sleep(6000);//TODO replace with Thread.Sleep(delay);
                        delay = ProcessDelay(delay, 60000 * 2); //currentDelay, 2 mins increase, 
                        //Try to publish
                        var pubMessageResult = TryPublishMessage(pubMessages[0]);
                        if (pubMessageResult.Status == Enums.MessageStatus.Completed)
                        {
                            pubServiceIsActive = true;
                            pubMessages.Remove(pubMessages[0]);//Removed process Item from List
                            //Get current list
                            ResetMessageStuckInSending();
                            pubMessages = _pubMessageRepository.GetAll().AsNoTracking().Where(x => x.Status == Enums.MessageStatus.Pending).AsNoTracking().Take(100).ToList();
                        }
                    }
                    foreach (var pubMessage in pubMessages.ToList())
                    {
                        //Try to publish
                        var pubMessageResult = TryPublishMessage(pubMessage);
                        if (pubMessageResult.Status != Enums.MessageStatus.Completed)
                        {
                            pubServiceIsActive = false;
                            keepRunningFlag = true;
                            break;
                        }
                        pubMessages.Remove(pubMessage);
                    }
                }
            }

            void ResetMessageStuckInSending()
            {
                //Reset all message stuck in sending for over 20 mins
                var twentyMinsAgo = DateTime.Now.AddMinutes(20);
                var stuckMessages = _pubMessageRepository.GetAll().Where(x => x.Status == Enums.MessageStatus.Sending && x.LastModificationTime <= twentyMinsAgo ||
                    x.Status == Enums.MessageStatus.Sending && x.CreationTime <= twentyMinsAgo && x.LastModificationTime == null).ToList();

                stuckMessages.ForEach(x => x.Status = Enums.MessageStatus.Pending);
                _unitOfWork.SaveChanges();
            }
        }

        private static long ProcessDelay(long currentDelay, int inc, long maxDelay = 60000 * 30)
        {
            return (maxDelay > (currentDelay + inc)) ? maxDelay : currentDelay + inc;
        }

        private PublishedMessage TryPublishMessage(PublishedMessage pubMessage)
        {
            var busMessage = new BusMessage
            {
                Data = pubMessage.Message,
                BusMessageType = (int)pubMessage.MessageType,
            };
            //update message in db to sending so other process would not rety this particular message
            //Or Use other means to ensure no other process can trigger this process
            pubMessage = _pubMessageRepository.GetAll().FirstOrDefault(x => x.Id == pubMessage.Id);

            if (pubMessage.Status != Enums.MessageStatus.Pending)
                return pubMessage;

            pubMessage.Status = Enums.MessageStatus.Sending;
            _pubMessageRepository.Update(pubMessage);
            _unitOfWork.SaveChanges();

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
