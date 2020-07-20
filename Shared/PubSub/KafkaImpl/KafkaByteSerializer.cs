using Confluent.Kafka;
using Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.PubSub.KafkaImpl
{
    public class KafkaByteSerializer<T> : ISerializer<T> where T : new()
    {
        public byte[] Serialize(T data, SerializationContext context)
        {
            return data.SerializeToBytes();
        }
    }
}
