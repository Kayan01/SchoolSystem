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
    public interface IBackgoundPublishService
    {
        void Publish(PublishedMessage message);
    }
    public class BackgroundPublishService : IBackgoundPublishService
    {
        private readonly IProducerClient<BusMessage> _producerClient;
        private IRepository<PublishedMessage, Guid> _pubMessageRepository;
        private IUnitOfWork _unitOfWork;
        private ILogger<BackgroundPublishService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private bool retryServiceIsRunning;

        public BackgroundPublishService(IServiceScopeFactory serviceScopeFactory,
            IProducerClient<BusMessage> producerClient)
        {
            using var scope = serviceScopeFactory.CreateScope();
            _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            _unitOfWork = scope.ServiceProvider.CreateScope().ServiceProvider.GetRequiredService<IUnitOfWork>();
            _pubMessageRepository = scope.ServiceProvider.CreateScope().ServiceProvider.GetRequiredService<IRepository<PublishedMessage, Guid>>();
            _logger = scope.ServiceProvider.GetRequiredService<ILogger<BackgroundPublishService>>();
        }

        public void Publish(PublishedMessage message)
        {
            var pubMessage = _pubMessageRepository.FirstOrDefault(x => x.Id == message.Id);

            try
            {
                var deliveryResult = _producerClient.Produce(message.Topic,
                        new BusMessage((int)message.MessageType, message.Message)).Result;

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

                /*if (!retryServiceIsRunning)
                    RetryPendingMessages();*///begin retry protocol
            }
        }
    }
}
