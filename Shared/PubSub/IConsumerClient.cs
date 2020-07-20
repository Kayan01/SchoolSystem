using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Shared.PubSub
{
    public interface IConsumerClient<T>
    {
        ConsumeResult<Ignore, T> Poll(CancellationToken cancellationToken);
        void Subscribe(List<string> topics);
        event Action<string> OnMessageRecieved;
    }
}
