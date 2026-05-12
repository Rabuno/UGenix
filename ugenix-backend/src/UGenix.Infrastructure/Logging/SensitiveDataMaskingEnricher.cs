using Serilog.Core;
using Serilog.Events;

namespace UGenix.Infrastructure.Logging;

public class SensitiveDataMaskingEnricher : ILogEventEnricher
{
    private static readonly string[] MaskedProperties = { "Password", "Token", "CreditCard", "Secret", "Authorization" };
    private const string MaskValue = "***MASKED***";

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        foreach (var property in logEvent.Properties)
        {
            if (IsSensitive(property.Key))
            {
                logEvent.AddOrUpdateProperty(new LogEventProperty(property.Key, new ScalarValue(MaskValue)));
            }
            else if (property.Value is StructureValue structure)
            {
                // Note: StructureValue is immutable, so we'd need to rebuild it if we wanted deep masking.
                // For MVP, we mask top-level sensitive keys.
            }
        }
    }

    private static bool IsSensitive(string propertyName)
    {
        return MaskedProperties.Any(p => propertyName.Contains(p, StringComparison.OrdinalIgnoreCase));
    }
}

