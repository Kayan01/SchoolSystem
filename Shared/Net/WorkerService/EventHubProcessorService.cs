using Microsoft.Extensions.Hosting;
using Shared.PubSub;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Net.WorkerService
{
    public class EventHubProcessorService : BackgroundService
    {
        //private readonly ILogger<MessageProcessorService> _logger;
        private readonly BoundedMessageChannel<BusMessage> _boundedMessageChannel;
        private readonly List<BusHandler> _busDelegate;

        public EventHubProcessorService(BoundedMessageChannel<BusMessage> boundedMessageChannel,
            Func<List<BusHandler>> busDelegate)
        {
            // _logger = logger;
            _boundedMessageChannel = boundedMessageChannel;
            _busDelegate = busDelegate();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var message = await _boundedMessageChannel.ReadAsync(stoppingToken);

                    if (message.Message.Value == null)
                        continue;

                    var busMsg = message.Message.Value;

                    if (busMsg == null)
                        continue;


                    foreach (var item in _busDelegate)
                    {
                        item(busMsg);
                    }
                }
                catch (OperationCanceledException)
                {
                    // Log an swallow as the while loop will end gracefully when cancellation has been requested
                    //_logger.OperationCancelledExceptionOccurred();
                }
                catch (Exception e)
                {
                    // If errors occur, we will probably send this to a poison queue, allow the message 
                    // to be deleted and continue processing other messages.
                    //_logger.ExceptionOccurred(ex);
                    // Note: Assumes no roll back is needed due to partial success for various processing tasks.
                }
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
        }
    }
}
