using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.PubSub
{
    public class PublishMessageBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public PublishMessageBackgroundService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await Task.Yield();

                    var backgroundPublishService = new BackgroundPublishService(_serviceScopeFactory);

                    await backgroundPublishService.RetryPublish();

                }
                catch (OperationCanceledException e)
                {

                }
                catch (Exception e)
                {
                }
            }
        }
    }
}
