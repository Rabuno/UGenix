namespace UGenix.Shared.Abstractions;

public record ConcurrencyMetadata(byte[] RowVersion);

public static class IdempotencyConstants
{
    public const string HeaderName = "X-Idempotency-Key";
}

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}

public interface ICurrentUser
{
    Guid UserId { get; }
    string Email { get; }
}

public interface IRequestContext
{
    ICurrentUser CurrentUser { get; }
}

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

public class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key, CancellationToken ct = default);
    Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken ct = default);
    Task RemoveAsync(string key, CancellationToken ct = default);
}

public interface IDistributedLock : IAsyncDisposable
{
    bool IsAcquired { get; }
}

public interface IDistributedLockFactory
{
    Task<IDistributedLock> AcquireAsync(string resource, TimeSpan? timeout = null, CancellationToken ct = default);
}

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hash);
}

