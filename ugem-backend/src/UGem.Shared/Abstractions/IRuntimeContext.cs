namespace UGem.Shared.Abstractions;

public interface IRuntimeContext
{
    Guid CorrelationId { get; }
    Guid RequestId { get; }
    string? TraceId { get; }
    string? TenantId { get; } // Multi-tenancy readiness
    string? Locale { get; }
    string? Timezone { get; }
}

public interface ICancellationProvider
{
    CancellationToken Token { get; }
}

public record SecurityAuditMetadata(
    string IpAddress,
    string UserAgent,
    string? DeviceFingerprint);

public interface ISecurityAuditService
{
    Task LogSecurityEventAsync(string eventName, SecurityAuditMetadata metadata);
}
