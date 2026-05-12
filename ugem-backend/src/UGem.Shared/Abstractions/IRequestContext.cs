namespace UGem.Shared.Abstractions;

public interface IRequestContext
{
    Guid CorrelationId { get; }
    string? UserAgent { get; }
    string? IpAddress { get; }
}

public interface ICurrentUser
{
    UserId Id { get; }
    string Email { get; }
    string Role { get; }
    bool IsAuthenticated { get; }
    IEnumerable<string> Permissions { get; }
}

public sealed class RequestContext : IRequestContext
{
    public RequestContext(Guid correlationId, string? userAgent, string? ipAddress)
    {
        CorrelationId = correlationId;
        UserAgent = userAgent;
        IpAddress = ipAddress;
    }

    public Guid CorrelationId { get; }
    public string? UserAgent { get; }
    public string? IpAddress { get; }
}
