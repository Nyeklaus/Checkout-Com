using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ThiagoCampos.CheckoutApi
{
    internal class SelfHealthCheck : IHealthCheck
    {
        Task<HealthCheckResult> IHealthCheck.CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(new HealthCheckResult(
                HealthStatus.Healthy,
                "Internal checks OK", 
                null, 
                new Dictionary<string, object>()
                {
                    { "MemmoryQueueSize", 0 }
                }
                ));
        }
    }
}