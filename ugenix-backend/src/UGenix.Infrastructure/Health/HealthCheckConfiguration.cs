using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace UGenix.Infrastructure.Health;

public static class HealthCheckConfiguration
{
    public static IServiceCollection AddUGenixHealthChecks(
        this IServiceCollection services,
        string dbConnectionString,
        string redisConnectionString)
    {
        services.AddHealthChecks()
            .AddNpgSql(dbConnectionString, name: "PostgreSQL", failureStatus: HealthStatus.Unhealthy)
            .AddRedis(redisConnectionString, name: "Redis", failureStatus: HealthStatus.Unhealthy)
            .AddCheck("API", () => HealthCheckResult.Healthy(), tags: new[] { "live" });

        return services;
    }
}

