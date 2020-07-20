using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Shared.PubSub
{
    public class BoundedMessageChannel<T>
    {
        private const int MaxMessagesInChannel = 250;
        private readonly Channel<ConsumeResult<Ignore, BusMessage>> _channel;
        private readonly ILogger<BoundedMessageChannel<BusMessage>> _logger;

        public BoundedMessageChannel(ILogger<BoundedMessageChannel<BusMessage>> logger)
        {
            _logger = logger;
            var options = new BoundedChannelOptions(MaxMessagesInChannel)
            {
                SingleReader = false,
                SingleWriter = true,

            };

            _channel = Channel.CreateBounded<ConsumeResult<Ignore, BusMessage>>(options);
        }

        public async ValueTask<ConsumeResult<Ignore, BusMessage>> ReadAsync(CancellationToken ct)
        {
            return await _channel.Reader.ReadAsync(ct);
        }

        public async Task WriteMessageAsync(ConsumeResult<Ignore, BusMessage> message,
         CancellationToken ct = default(CancellationToken))
        {
            if (message != null && await _channel.Writer.WaitToWriteAsync(ct) && !ct.IsCancellationRequested)
            {
                _channel.Writer.TryWrite(message);
            }
        }

        public void CompleteWriter(Exception ex = null) => _channel.Writer.Complete(ex);

        public bool TryCompleteWriter(Exception ex = null) => _channel.Writer.TryComplete(ex);
    }
}