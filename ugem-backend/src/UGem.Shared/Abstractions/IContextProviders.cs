using UGem.Shared.Abstractions;

namespace UGem.Shared.Abstractions;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}

public interface ICurrentUser
{
    UserId? Id { get; }
    string? Email { get; }
    bool IsAuthenticated { get; }
}

public interface IRequestContext
{
    Guid CorrelationId { get; }
    string? IpAddress { get; }
}

public class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
