using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using UGenix.Shared.Abstractions;
using UGenix.Infrastructure.Caching;
using UGenix.Infrastructure.Security;
using UGenix.Infrastructure.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UGenix.Infrastructure.Options;

namespace UGenix.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Options Mapping
        services.AddOptions<JwtOptions>()
            .BindConfiguration("Jwt")
            .ValidateDataAnnotations()
            .ValidateOnStart();

        // Caching
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
        });
        services.AddSingleton<ICacheService, RedisCacheService>();

        // Auth
        services.AddSingleton<IJwtService, JwtService>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var jwtOptions = configuration.GetSection("Jwt").Get<JwtOptions>();
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions?.Issuer,
                    ValidAudience = jwtOptions?.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions?.Secret ?? string.Empty))
                };
            });

        return services;
    }
}

