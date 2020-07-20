using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.PubSub.KafkaImpl
{
    public interface IKafkaFactoryResolver
    {
        IConsumerClient<T> GetKafkaConsumer<T>(string groupId = null) where T : new();
        IProducerClient<T> GetKafkaProducer<T>() where T : new();
    }
}
