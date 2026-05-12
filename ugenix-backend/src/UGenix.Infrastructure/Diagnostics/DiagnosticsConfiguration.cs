using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace UGenix.Infrastructure.Diagnostics;

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
                .CreateLogger());
        });

        // 2. OpenTelemetry Tracing (core instrumentation only - no beta packages)
        services.AddOpenTelemetry()
            .WithTracing(tracing => tracing
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName))
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddOtlpExporter());

        return services;
    }
}

