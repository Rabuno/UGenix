using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace UGem.Infrastructure.Logging;

public class SecurityBoundaryEnricher : ILogEventEnricher
{
    private static readonly string[] ForbiddenKeys = { 
        "SecretKey", "ActiveSecretKey", "ApiSecret", "Password", "ConnectionString", "ApiKey", 
        "AnonKey", "ServiceRoleKey", "JwtSecret", "Url", "Token",
        "Authorization", "Cookie", "Set-Cookie", "X-Api-Key"
    };

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        foreach (var prop in logEvent.Properties)
        {
            if (ForbiddenKeys.Any(k => prop.Key.Contains(k, StringComparison.OrdinalIgnoreCase)))
            {
                logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty(prop.Key, "[MASKED_BY_GOVERNANCE]"));
            }
        }
    }
}
