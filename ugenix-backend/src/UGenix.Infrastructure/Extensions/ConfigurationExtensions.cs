using Serilog;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using UGenix.Infrastructure.Options;

namespace UGenix.Infrastructure.Extensions;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddProductionConfiguration(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        // 1. Log Sources for Traceability (Diagnostic-safe)
        if (configuration is IConfigurationRoot root)
        {
            Log.Information("UGenix Platform: Initializing Configuration Providers...");
            foreach (var provider in root.Providers)
            {
                Log.Information("Config Provider Active: {ProviderName}", provider.ToString());
            }
        }

        // 2. Critical Configuration (Fail-Fast, No Defaults)
        services.AddOptions<PostgresOptions>()
            .Configure(options => {
                options.Url = configuration["POSTGRES_URL"] ?? string.Empty;
                options.Host = configuration["POSTGRES_HOST"] ?? string.Empty;
                options.Database = configuration["POSTGRES_DATABASE"] ?? string.Empty;
                options.User = configuration["POSTGRES_USER"] ?? string.Empty;
                options.Password = configuration["POSTGRES_PASSWORD"] ?? string.Empty;
            })
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<SupabaseOptions>()
            .Configure(options => {
                options.Url = configuration["SUPABASE_URL"] ?? string.Empty;
                options.AnonKey = configuration["SUPABASE_ANON_KEY"] ?? string.Empty;
                options.ServiceRoleKey = configuration["SUPABASE_SERVICE_ROLE_KEY"] ?? string.Empty;
                options.JwtSecret = configuration["SUPABASE_JWT_SECRET"] ?? string.Empty;
            })
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<JwtOptions>()
            .Bind(configuration.GetSection(nameof(JwtOptions)))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        // 3. Sensitive Configuration (Support Hot Reload / IOptionsMonitor)
        services.AddOptions<CloudinaryOptions>()
            .Bind(configuration.GetSection(nameof(CloudinaryOptions)))
            .ValidateDataAnnotations();

        services.AddOptions<MailOptions>()
            .Bind(configuration.GetSection(nameof(MailOptions)))
            .ValidateDataAnnotations();

        services.AddOptions<GoogleAuthOptions>()
            .Bind(configuration.GetSection("GoogleAuth"))
            .ValidateDataAnnotations();

        // 4. Infrastructure & Redis (With local fallbacks if safe)
        services.AddOptions<RedisOptions>()
            .Configure(options => {
                options.ConnectionString = configuration["REDIS_URL"] 
                                           ?? configuration["RedisOptions__ConnectionString"] 
                                           ?? "localhost:6379";
            })
            .ValidateDataAnnotations();

        services.AddOptions<ObservabilityOptions>()
            .Bind(configuration.GetSection("Observability"))
            .ValidateDataAnnotations();

        return services;
    }
}

