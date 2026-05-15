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
        // Sync Postgres for AddPersistence (uses GetConnectionString("Database"))
        string? dbUrl = configuration["POSTGRES_URL"] ?? configuration["DATABASE_URL"] ?? configuration["DATABASE_PRIVATE_URL"];
        if (configuration is IConfigurationRoot configurationRoot && !string.IsNullOrEmpty(dbUrl))
        {
            configurationRoot["ConnectionStrings:Database"] = dbUrl;
        }

        services.AddOptions<PostgresOptions>()
            .Bind(configuration.GetSection("Postgres"))
            .Configure(options => {
                options.Url = configuration["POSTGRES_URL"] 
                    ?? configuration["DATABASE_URL"] 
                    ?? configuration["DATABASE_PRIVATE_URL"]
                    ?? configuration.GetConnectionString("Database")
                    ?? options.Url;
                
                options.Host = configuration["POSTGRES_HOST"] ?? options.Host;
                options.Database = configuration["POSTGRES_DATABASE"] ?? options.Database;
                options.User = configuration["POSTGRES_USER"] ?? options.User;
                options.Password = configuration["POSTGRES_PASSWORD"] ?? options.Password;
            })
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<SupabaseOptions>()
            .Bind(configuration.GetSection("Supabase"))
            .Configure(options => {
                options.Url = configuration["SUPABASE_URL"] ?? options.Url;
                options.AnonKey = configuration["SUPABASE_ANON_KEY"] ?? options.AnonKey;
                options.ServiceRoleKey = configuration["SUPABASE_SERVICE_ROLE_KEY"] ?? options.ServiceRoleKey;
                options.JwtSecret = configuration["SUPABASE_JWT_SECRET"] ?? options.JwtSecret;
            })
            .ValidateDataAnnotations();

        services.AddOptions<JwtOptions>()
            .Bind(configuration.GetSection("Jwt")) 
            .Configure(options => {
                options.Secret = configuration["JWT_SECRET"] ?? options.Secret;
                options.Issuer = configuration["JWT_ISSUER"] ?? options.Issuer;
                options.Audience = configuration["JWT_AUDIENCE"] ?? options.Audience;
                
                if (int.TryParse(configuration["JWT_ACCESS_TOKEN_EXPIRATION_MINUTES"], out int accessExp))
                    options.AccessTokenExpirationMinutes = accessExp;
                
                if (int.TryParse(configuration["JWT_REFRESH_TOKEN_EXPIRATION_DAYS"], out int refreshExp))
                    options.RefreshTokenExpirationDays = refreshExp;
            })
            .ValidateDataAnnotations()
            .ValidateOnStart();

        // 3. Sensitive Configuration (Support Hot Reload / IOptionsMonitor)
        services.AddOptions<CloudinaryOptions>()
            .Bind(configuration.GetSection("Cloudinary"))
            .Configure(options => {
                options.CloudName = configuration["CLOUDINARY_CLOUD_NAME"] ?? options.CloudName;
                options.ApiKey = configuration["CLOUDINARY_API_KEY"] ?? options.ApiKey;
                options.ApiSecret = configuration["CLOUDINARY_API_SECRET"] ?? options.ApiSecret;
            })
            .ValidateDataAnnotations();

        services.AddOptions<MailOptions>()
            .Bind(configuration.GetSection("Mail"))
            .Configure(options => {
                options.Mail = configuration["MAIL_USER"] ?? options.Mail;
                options.Password = configuration["MAIL_PASSWORD"] ?? options.Password;
                options.Host = configuration["MAIL_HOST"] ?? options.Host;
                
                if (int.TryParse(configuration["MAIL_PORT"], out int port))
                    options.Port = port;
                
                options.DisplayName = configuration["MAIL_DISPLAY_NAME"] ?? options.DisplayName;
            })
            .ValidateDataAnnotations();

        services.AddOptions<GoogleAuthOptions>()
            .Bind(configuration.GetSection("GoogleAuth"))
            .Configure(options => {
                options.ClientId = configuration["GOOGLE_CLIENT_ID"] ?? options.ClientId;
            })
            .ValidateDataAnnotations();

        // 4. Infrastructure & Redis (With local fallbacks if safe)
        services.AddOptions<RedisOptions>()
            .Bind(configuration.GetSection("Redis"))
            .Configure(options => {
                options.ConnectionString = configuration["REDIS_URL"] 
                                           ?? configuration["REDIS_PRIVATE_URL"]
                                           ?? configuration.GetConnectionString("Redis")
                                           ?? options.ConnectionString;
            })
            .ValidateDataAnnotations();

        services.AddOptions<ObservabilityOptions>()
            .Bind(configuration.GetSection("Observability"))
            .ValidateDataAnnotations();

        return services;
    }
}

