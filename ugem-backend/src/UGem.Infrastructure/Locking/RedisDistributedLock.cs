using RedLockNet;
using UGem.Application.Abstractions;

namespace UGem.Infrastructure.Locking;

public class RedisDistributedLock : IDistributedLock
{
    private readonly IDistributedLockFactory _lockFactory;

    public RedisDistributedLock(IDistributedLockFactory lockFactory)
    {
        _lockFactory = lockFactory;
    }

    public async Task<bool> AcquireLockAsync(string resource, TimeSpan expiry, CancellationToken ct = default)
    {
        var @lock = await _lockFactory.CreateLockAsync(resource, expiry);
        return @lock.IsAcquired;
    }

    // Actual production implementation would return an IAsyncDisposable lock handle
}
