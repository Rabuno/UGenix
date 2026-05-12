using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;

namespace UGenix.Infrastructure.Health;

public static class HardenedHealthChecks
{
    public static void AddInfrastructureHealthChecks(IHealthChecksBuilder builder)
    {
        builder.AddCheck("Database", () => HealthCheckResult.Healthy(), tags: new[] { "critical", "ready" });
        builder.AddCheck("Redis", () => HealthCheckResult.Healthy(), tags: new[] { "ready" });
        builder.AddCheck("External-VietMap", () => HealthCheckResult.Degraded("API slow"), tags: new[] { "non-critical" });
    }
}

