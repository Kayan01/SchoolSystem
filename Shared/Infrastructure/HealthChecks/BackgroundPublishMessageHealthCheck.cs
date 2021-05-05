using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Infrastructure.HealthChecks
{
    public class BackgroundPublishMessageHealthCheck : IHealthCheck
    {
        private volatile bool _publishTaskRunning= true;

        public string Name => "slow_dependency_check";

        public bool PublishTaskRunning
        {
            get => _publishTaskRunning;
            set => _publishTaskRunning = value;
        }

        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (PublishTaskRunning)
            {
                return Task.FromResult(
                    HealthCheckResult.Healthy("The background publish message service is running."));
            }

            return Task.FromResult(
                HealthCheckResult.Unhealthy("The background publish message service is not running."));
        }
    }
}
