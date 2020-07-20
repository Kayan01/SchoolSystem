using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Shared.PubSub;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Net.WorkerService
{
    public class EventHubReaderService : BackgroundService
    {
        private readonly BoundedMessageChannel<BusMessage> _boundedMessageChannel;
        private readonly IConsumerClient<BusMessage> _consumer;
        private readonly int _delay;

        public EventHubReaderService(BoundedMessageChannel<BusMessage> boundedMessageChannel,
            IConsumerClient<BusMessage> consumer, IConfiguration configuration)
        {
            _consumer = consumer;
            _boundedMessageChannel = boundedMessageChannel;
            _delay = configuration.GetValue<int>("CheckUpdateDelayMillSec");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await Task.Yield();//This Fixes the issues of consumer blocking startup
                    var message = _consumer.Poll(stoppingToken);

                    if (message == null)
                    {
                        continue;
                    }
                    await _boundedMessageChannel.WriteMessageAsync(message, stoppingToken);
                    await Task.Delay(_delay, stoppingToken);
                }
                catch (OperationCanceledException e)
                {
                    // Log an swallow as the while loop will end gracefully when cancellation has been requested
                    //_logger.OperationCancelledExceptionOccurred();
                }
                catch (Exception e)
                {
                }
            }
        }
    }
}