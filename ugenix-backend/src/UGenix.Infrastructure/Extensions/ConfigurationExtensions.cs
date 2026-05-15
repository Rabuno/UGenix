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
                options.Url = configuration["POSTGRES_URL"] ?? configuration["DATABASE_URL"] ?? string.Empty;
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
            .Configure(options => {
                options.Secret = configuration["JWT_SECRET"] ?? configuration["Jwt:Secret"] ?? string.Empty;
                options.Issuer = configuration["JWT_ISSUER"] ?? configuration["Jwt:Issuer"] ?? string.Empty;
                options.Audience = configuration["JWT_AUDIENCE"] ?? configuration["Jwt:Audience"] ?? string.Empty;
                
                if (int.TryParse(configuration["JWT_ACCESS_TOKEN_EXPIRATION_MINUTES"], out int accessExp))
                    options.AccessTokenExpirationMinutes = accessExp;
                
                if (int.TryParse(configuration["JWT_REFRESH_TOKEN_EXPIRATION_DAYS"], out int refreshExp))
                    options.RefreshTokenExpirationDays = refreshExp;
            })
            .Bind(configuration.GetSection("Jwt")) 
            .ValidateDataAnnotations()
            .ValidateOnStart();

        // 3. Sensitive Configuration (Support Hot Reload / IOptionsMonitor)
        services.AddOptions<CloudinaryOptions>()
            .Configure(options => {
                options.CloudName = configuration["CLOUDINARY_CLOUD_NAME"] ?? configuration["Cloudinary:CloudName"] ?? string.Empty;
                options.ApiKey = configuration["CLOUDINARY_API_KEY"] ?? configuration["Cloudinary:ApiKey"] ?? string.Empty;
                options.ApiSecret = configuration["CLOUDINARY_API_SECRET"] ?? configuration["Cloudinary:ApiSecret"] ?? string.Empty;
            })
            .Bind(configuration.GetSection("Cloudinary"))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<MailOptions>()
            .Configure(options => {
                options.Mail = configuration["MAIL_USER"] ?? configuration["Mail:Mail"] ?? string.Empty;
                options.Password = configuration["MAIL_PASSWORD"] ?? configuration["Mail:Password"] ?? string.Empty;
                options.Host = configuration["MAIL_HOST"] ?? configuration["Mail:Host"] ?? string.Empty;
                
                if (int.TryParse(configuration["MAIL_PORT"], out int port))
                    options.Port = port;
                
                options.DisplayName = configuration["MAIL_DISPLAY_NAME"] ?? configuration["Mail:DisplayName"] ?? "UGenix";
            })
            .Bind(configuration.GetSection("Mail"))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<GoogleAuthOptions>()
            .Configure(options => {
                options.ClientId = configuration["GOOGLE_CLIENT_ID"] ?? configuration["GoogleAuth:ClientId"] ?? string.Empty;
            })
            .Bind(configuration.GetSection("GoogleAuth"))
            .ValidateDataAnnotations();

        // 4. Infrastructure & Redis (With local fallbacks if safe)
        services.AddOptions<RedisOptions>()
            .Configure(options => {
                options.ConnectionString = configuration["REDIS_URL"] 
                                           ?? configuration["RedisOptions__ConnectionString"] 
                                           ?? "localhost:6379";
            })
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<ObservabilityOptions>()
            .Bind(configuration.GetSection("Observability"))
            .ValidateDataAnnotations();

        return services;
    }
}

