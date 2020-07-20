using Confluent.Kafka;
using Microsoft.AspNetCore.Hosting;
using Microsoft.DotNet.PlatformAbstractions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shared.PubSub.KafkaImpl
{
    public class ProducerClient<T> : IProducerClient<T> where T : new()
    {
        private readonly IProducer<Null, T> _producer;
        private readonly IConfiguration _globalconf;
        private readonly string _environment;

        public ProducerClient(string environment, IProducer<Null, T> producer)
        {
            _producer = producer;
            _environment = environment;
        }

        public ProducerClient(IWebHostEnvironment env, IConfiguration globalconf)
        {
            _globalconf = globalconf;

            string sslCaLocation = $@"{ApplicationEnvironment.ApplicationBasePath}{globalconf.GetValue<string>("Kafka:Cert")}";

            var producerConfig = new ProducerConfig
            {
                BootstrapServers = globalconf.GetValue<string>("Kafka:BootstrapServers"),
                ApiVersionFallbackMs = 0,
                //Debug = "security,broker,protocol"
            };
            if (globalconf.GetValue<bool>("Kafka:UseSSL"))
            {
                producerConfig.SecurityProtocol = SecurityProtocol.SaslSsl;
                producerConfig.SaslMechanism = SaslMechanism.Plain;
                producerConfig.SslCaLocation = sslCaLocation;
            }

            if (globalconf.GetValue<bool>("Kafka:RequireLogin"))
            {
                producerConfig.SaslUsername = globalconf.GetValue<string>("Kafka:Username");
                producerConfig.SaslPassword = globalconf.GetValue<string>("Kafka:Password");
            }

            //if (env.IsDevelopment())
            //{
            //    producerConfig = new ProducerConfig
            //    {
            //        BootstrapServers = globalconf.GetValue<string>("Kafka:BootstrapServers"),
            //        SecurityProtocol = SecurityProtocol.SaslSsl,
            //        SaslMechanism = SaslMechanism.Plain,
            //        SaslUsername = globalconf.GetValue<string>("Kafka:Username"),
            //        SaslPassword = globalconf.GetValue<string>("Kafka:Password"),
            //        SslCaLocation = sslCaLocation,
            //        ApiVersionFallbackMs = 0,
            //        Debug = "security,broker,protocol"
            //    };
            //    producerConfig = new ProducerConfig { BootstrapServers = "localhost:9092" };
            //}
            //else
            //{
            //    producerConfig = new ProducerConfig
            //    {
            //        BootstrapServers = globalconf.GetValue<string>("Kafka:BootstrapServers"),
            //        SecurityProtocol = SecurityProtocol.SaslSsl,
            //        SaslMechanism = SaslMechanism.Plain,
            //        SaslUsername = globalconf.GetValue<string>("Kafka:Username"),
            //        SaslPassword = globalconf.GetValue<string>("Kafka:Password"),
            //        SslCaLocation = sslCaLocation,
            //        ApiVersionFallbackMs = 0,
            //    };
            //}

            var producerBuilder = new ProducerBuilder<Null, T>(producerConfig);
            producerBuilder.SetValueSerializer(new KafkaByteSerializer<T>());
            _producer = producerBuilder.Build();

            _environment = _globalconf.GetSection("Kafka").GetValue<string>("Environment").ToString();
        }

        public async Task<DeliveryResult<Null, T>> Produce(string topic, T message)
        {
            topic = $"{_environment}_{topic}";
            var msg = new Message<Null, T>();
            msg.Value = message;
            return await _producer.ProduceAsync(topic, msg);
        }
    }
}
