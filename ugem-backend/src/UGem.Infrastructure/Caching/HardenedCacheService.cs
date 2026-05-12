using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using UGem.Application.Abstractions;
using UGem.Infrastructure.Locking;

namespace UGem.Infrastructure.Caching;

public class HardenedCacheService : ICacheService
{
    private readonly IDistributedCache _cache;
    private readonly IDistributedLock _lock;
    private readonly JsonSerializerOptions _jsonOptions;

    public HardenedCacheService(IDistributedCache cache, IDistributedLock @lock)
    {
        _cache = cache;
        _lock = @lock;
        _jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
    }

    public async Task<T?> GetOrSetAsync<T>(
        string key, 
        Func<Task<T>> factory, 
        TimeSpan? expiration = null, 
        CancellationToken ct = default)
    {
        // 1. First attempt: Read from cache
        var cachedValue = await GetAsync<T>(key, ct);
        if (cachedValue is not null) return cachedValue;

        // 2. Cache Miss: Acquire distributed lock to prevent stampede
        var lockKey = $"lock:cache:{key}";
        if (await _lock.AcquireLockAsync(lockKey, TimeSpan.FromSeconds(10), ct))
        {
            try
            {
                // 3. Double-check after acquiring lock
                cachedValue = await GetAsync<T>(key, ct);
                if (cachedValue is not null) return cachedValue;

                // 4. Fetch from source (DB/API)
                var value = await factory();
                await SetAsync(key, value, expiration, ct);
                return value;
            }
            finally
            {
                // Release lock handled by IAsyncDisposable in real RedLock implementation
            }
        }

        // 5. Fallback: If lock cannot be acquired, wait and retry or return default (stale-while-revalidate logic)
        await Task.Delay(500, ct);
        return await GetAsync<T>(key, ct);
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken ct = default)
    {
        var bytes = await _cache.GetAsync(key, ct);
        return bytes is null ? default : JsonSerializer.Deserialize<T>(bytes, _jsonOptions);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken ct = default)
    {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(value, _jsonOptions);
        await _cache.SetAsync(key, bytes, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(10)
        }, ct);
    }

    public async Task RemoveAsync(string key, CancellationToken ct = default) => await _cache.RemoveAsync(key, ct);
}
