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
using UGenix.Application.Abstractions;
using UGenix.Persistence;

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

        var redisConnectionString = configuration.GetConnectionString("Redis") ?? string.Empty;
        if (!string.IsNullOrEmpty(redisConnectionString) && redisConnectionString.StartsWith("redis://"))
        {
            var uri = new Uri(redisConnectionString);
            var userInfo = uri.UserInfo.Split(':');
            var password = userInfo.Length > 1 ? userInfo[1] : string.Empty;
            redisConnectionString = $"{uri.Host}:{uri.Port}";
            if (!string.IsNullOrEmpty(password))
            {
                redisConnectionString += $",password={password}";
            }
        }

        // Caching
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnectionString;
        });
        services.AddSingleton<ICacheService, RedisCacheService>();

        // Security
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IIdentityService, IdentityService>();

        // Shared Services
        services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
        services.AddScoped<ICurrentUser, WebCurrentUser>();

        // Auth
        services.AddSingleton<IJwtService, JwtService>();
        services.AddHttpContextAccessor();
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

        // Health Checks
        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>(name: "postgres")
            .AddRedis(redisConnectionString, name: "redis");

        return services;
    }
}

public class WebCurrentUser(Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    public Guid UserId
    {
        get
        {
            var userIdClaim = httpContextAccessor.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(userIdClaim, out var guid) ? guid : Guid.Empty;
        }
    }

    public string Email => httpContextAccessor.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value ?? string.Empty;
}

