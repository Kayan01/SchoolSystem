using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Infrastructure.HealthChecks;
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
        private readonly BackgroundPublishMessageHealthCheck _backgroundPublishMessageHealthCheck;

        public PublishMessageBackgroundService(IServiceScopeFactory serviceScopeFactory, BackgroundPublishMessageHealthCheck backgroundPublishMessageHealthCheck)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _backgroundPublishMessageHealthCheck = backgroundPublishMessageHealthCheck;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await Task.Yield();

                    var backgroundPublishService = new BackgroundPublishService(_serviceScopeFactory);

                    _backgroundPublishMessageHealthCheck.PublishTaskRunning = true;

                    await backgroundPublishService.RetryPublish();

                    
                }
                catch (OperationCanceledException e)
                {

                    _backgroundPublishMessageHealthCheck.PublishTaskRunning = false;
                }
                catch (Exception e)
                {

                    _backgroundPublishMessageHealthCheck.PublishTaskRunning = false;
                }

            }


            _backgroundPublishMessageHealthCheck.PublishTaskRunning = false;
        }


        public override Task StopAsync(CancellationToken cancellationToken)
        {

            _backgroundPublishMessageHealthCheck.PublishTaskRunning = false;
            return base.StopAsync(cancellationToken);
        }
    }
}
