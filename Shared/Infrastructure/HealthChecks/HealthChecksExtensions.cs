using Confluent.Kafka;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Infrastructure.HealthChecks
{
    public static class HealthChecksExtensions
    {
        public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services, IConfiguration Configuration, string title = "Base Template API")
        {
            // Registers required services for health checks

            // Add a health check for the SQL Server database
            services.AddHealthChecks()
                .AddSqlServer(
                Configuration.GetConnectionString("Default"),
                name: $"{title}-db-check",
                tags: new string[] { title.ToLower() });


            // Add a health check for a Kafka connection
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = Configuration.GetValue<string>("Kafka:BootstrapServers"),
                ApiVersionFallbackMs = 0,
                MessageSendMaxRetries = 0,
                MessageTimeoutMs = 1500,
                RequestTimeoutMs = 1500,
                SocketTimeoutMs = 1500,
                MetadataRequestTimeoutMs = 1500,
            };
            services.AddHealthChecks()
                .AddKafka(producerConfig, name:$"Kafka on {title}", tags: new string[] { $"{title}-kafka" });

            services.AddSingleton<BackgroundPublishMessageHealthCheck>();

            services.AddHealthChecks()
                .AddCheck<BackgroundPublishMessageHealthCheck>(
                $"{title}-Background Publish Service",
                failureStatus: HealthStatus.Unhealthy,
                tags: new[] { "ready" });


            return services;
        }

        public static void UseCustomHealthChecksAPI(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/hc", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
        }
    }
}
