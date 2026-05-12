using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace UGem.Infrastructure.Diagnostics;

public static class DiagnosticsConfiguration
{
    public static IServiceCollection AddObservability(
        this IServiceCollection services,
        string serviceName)
    {
        // 1. Serilog Setup (Structured Logging)
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog(new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.Seq("http://ugem-seq:5341") // Centralized logs
                .CreateLogger());
        });

        // 2. OpenTelemetry Tracing
        services.AddOpenTelemetry()
            .WithTracing(tracing => tracing
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName))
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddEntityFrameworkCoreInstrumentation()
                .AddRedisInstrumentation()
                .AddOtlpExporter());

        return services;
    }
}
