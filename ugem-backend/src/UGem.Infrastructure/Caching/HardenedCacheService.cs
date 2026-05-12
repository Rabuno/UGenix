using UGem.Shared.Abstractions;

namespace UGem.Infrastructure.Caching;

public class HardenedCacheService : ICacheService
{
    private readonly ICacheService _inner;
    private readonly IDistributedLockFactory _lockFactory;

    public HardenedCacheService(ICacheService inner, IDistributedLockFactory lockFactory)
    {
        _inner = inner;
        _lockFactory = lockFactory;
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken ct = default)
    {
        return await _inner.GetAsync<T>(key, ct);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken ct = default)
    {
        await using var @lock = await _lockFactory.AcquireAsync(key, ct: ct);
        if (@lock.IsAcquired)
        {
            await _inner.SetAsync(key, value, expiration, ct);
        }
    }

    public async Task RemoveAsync(string key, CancellationToken ct = default)
    {
        await _inner.RemoveAsync(key, ct);
    }
}
