using Confluent.Kafka;
using Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.PubSub.KafkaImpl
{
    public class KafkaByteDeserializer<T> : IDeserializer<T> where T : new()
    {

        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            var d = data.ToArray().Deserialize<T>();
            return d;
        }

    }
}
