using Microsoft.Extensions.Caching.Hybrid;

namespace SharedKernel;


public interface IHybridCacheService
{
    Task<Result<T>> GetOrCreateAsync<T>(string key, Func<CancellationToken, ValueTask<T>> factory, TimeSpan expiration = default);
    Task<Result<bool>> SetAsync<T>(string Key, T subscription, TimeSpan expiration = default);

    Task RemoveAsync(string Key);
}

public sealed class HybridCacheService : IHybridCacheService
{
    private readonly HybridCache _hybridCache;
    private HybridCacheEntryOptions hybridCacheEntryOptions;

    public HybridCacheService(HybridCache hybridCache)
    {
        _hybridCache = hybridCache;
        hybridCacheEntryOptions = new HybridCacheEntryOptions { Expiration = TimeSpan.FromHours(24), LocalCacheExpiration = TimeSpan.FromHours(24) };
    }




    public async Task<Result<T>> GetOrCreateAsync<T>(string key, Func<CancellationToken, ValueTask<T>> factory, TimeSpan expiration = default)
    {
        if (expiration != default)
            hybridCacheEntryOptions = new HybridCacheEntryOptions { Expiration = expiration, LocalCacheExpiration = expiration };

        var result = await _hybridCache.GetOrCreateAsync(key, factory, hybridCacheEntryOptions);
        if (result is null)
            return Error.NullValue;

        return result;
    }

    public async Task<Result<bool>> SetAsync<T>(string Key, T subscription, TimeSpan expiration = default)
    {
        if (expiration != default)
            hybridCacheEntryOptions = new HybridCacheEntryOptions { Expiration = expiration, LocalCacheExpiration = expiration };
        await _hybridCache.SetAsync(Key, subscription, hybridCacheEntryOptions);
        return true;
    }
    
    public async Task RemoveAsync(string Key) =>
        await _hybridCache.RemoveAsync(Key);
    
    
    
}