using Confluent.Kafka;
using Microsoft.DotNet.PlatformAbstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Shared.PubSub.KafkaImpl
{
    public class KafkaFactoryResolver : IKafkaFactoryResolver
    {
        private readonly IServiceProvider _serviceProvider;
        public KafkaFactoryResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IConsumerClient<T> GetKafkaConsumer<T>(string groupId = null) where T : new()
        {
            if (string.IsNullOrWhiteSpace(groupId))
            {
                return _serviceProvider.GetService<IConsumerClient<T>>();
            }

            var globalconf = _serviceProvider.GetService<IConfiguration>();

            string sslCaLocation = $@"{ApplicationEnvironment.ApplicationBasePath}{globalconf.GetValue<string>("Kafka:Cert")}";

            var consumerConfig = new ConsumerConfig
            {
                GroupId = groupId,
                BootstrapServers = globalconf.GetValue<string>("Kafka:BootstrapServers"),
                //Debug = "security,broker,protocol",
                ApiVersionRequestTimeoutMs = 60000,
                AutoOffsetReset = AutoOffsetReset.Latest,
            };

            if (globalconf.GetValue<bool>("Kafka:UseSSL"))
            {
                consumerConfig.SecurityProtocol = SecurityProtocol.SaslSsl;
                consumerConfig.SaslMechanism = SaslMechanism.Plain;
                consumerConfig.SslCaLocation = sslCaLocation;
            }

            if (globalconf.GetValue<bool>("Kafka:RequireLogin"))
            {
                consumerConfig.SaslUsername = "$ConnectionString";
                consumerConfig.SaslPassword = globalconf.GetValue<string>("Kafka:Password");
            }

            var topics = globalconf.GetSection("Kafka").GetValue<string>("Topics").ToString().Split(",");
            var environment = globalconf.GetSection("Kafka").GetValue<string>("Environment").ToString();
            //Append Env to Topics
            topics = topics.Select(x => $"{environment}_{x}").ToArray();

            var consumerBuilder = new ConsumerBuilder<Ignore, T>(consumerConfig);
            consumerBuilder.SetValueDeserializer(new KafkaByteDeserializer<T>());
            var consumer = consumerBuilder.Build();
            consumer.Subscribe(topics);

            var env = globalconf.GetSection("Kafka").GetValue<string>("Environment").ToString(); ;
            var consumerClient = new ConsumerClient<T>(env, consumer);
            return consumerClient;
        }

        public IProducerClient<T> GetKafkaProducer<T>() where T : new()
        {
            return _serviceProvider.GetService<IProducerClient<T>>();

            //var globalconf = _serviceProvider.GetService<IConfiguration>();

            //string sslCaLocation = $@"{ApplicationEnvironment.ApplicationBasePath}{globalconf.GetValue<string>("Kafka:Cert")}";

            //var producerConfig = new ProducerConfig
            //{
            //    BootstrapServers = globalconf.GetValue<string>("Kafka:BootstrapServers"),
            //    ApiVersionFallbackMs = 0,
            //    //Debug = "security,broker,protocol"
            //};

            //if (globalconf.GetValue<bool>("Kafka:UseSSL"))
            //{
            //    producerConfig.SecurityProtocol = SecurityProtocol.SaslSsl;
            //    producerConfig.SaslMechanism = SaslMechanism.Plain;
            //    producerConfig.SslCaLocation = sslCaLocation;
            //}

            //if (globalconf.GetValue<bool>("Kafka:RequireLogin"))
            //{
            //    producerConfig.SaslUsername = globalconf.GetValue<string>("Kafka:Username");
            //    producerConfig.SaslPassword = globalconf.GetValue<string>("Kafka:Password");
            //}

            //var producerBuilder = new ProducerBuilder<Null, T>(producerConfig);
            //producerBuilder.SetValueSerializer(new KafkaByteSerializer<T>());
            //var producer = producerBuilder.Build();

            //var env = globalconf.GetSection("Kafka").GetValue<string>("Environment").ToString(); ;
            //var producerClient = new ProducerClient<T>(env, producer);
            //return producerClient;
        }
    }
}
