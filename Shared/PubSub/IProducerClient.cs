using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shared.PubSub
{
    public interface IProducerClient<T>
    {
        Task<DeliveryResult<Null, T>> Produce(string topic, T message);

    }
}
