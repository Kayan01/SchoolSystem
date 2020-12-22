using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.Entities;
using System;
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

        public PublishService(IProducerClient<BusMessage> producerClient,
            IRepository<PublishedMessage, Guid> pubMessageRepository,
            IServiceScopeFactory serviceScopeFactory,
            ILogger<PublishService> logger,
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _pubMessageRepository = pubMessageRepository;
            _producerClient = producerClient;
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
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
        }

        private void Publish(PublishedMessage message)
        {
            var scope = _serviceScopeFactory.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var pubMessageRepository = scope.ServiceProvider.GetRequiredService<IRepository<PublishedMessage, Guid>>();
            var pubMessage = pubMessageRepository.FirstOrDefault(x => x.Id == message.Id);
            var producerClient = scope.ServiceProvider.GetRequiredService<IProducerClient<BusMessage>>();

            try
            {
                var deliveryResult = producerClient.Produce(message.Topic, 
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
                unitOfWork.SaveChanges();
            }
        }
    }
}
