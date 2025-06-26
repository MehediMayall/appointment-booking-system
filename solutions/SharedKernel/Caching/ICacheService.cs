using StackExchange.Redis;

namespace SharedKernel;

public interface ICacheService
{
    IDatabase GetDatabase();
    Task<Result<T?>> GetAsync<T>(string Key, CancellationToken cancellationToken = default) where T: class;
    Task<Result<T?>> GetOrCreateAsync<T>(string Key, Func<Task<T>> NonCached) where T: class;
    Task<Result<T?>> GetOrCreateAsync<T>(string Key, Func<Task<T>> NonCached, TimeSpan timeSpan = default, CancellationToken cancellationToken = default) where T: class;
    Task<Result<bool>> SetAsync<T>(string Key, T Value) where T: class;
    Task<Result<bool>> SetAsync<T>(string Key, T Value, TimeSpan timeSpan = default,  CancellationToken cancellationToken = default) where T: class;

    Task<Result<List<T?>>> GetListAsync<T>(string Key, CancellationToken cancellationToken = default) where T: class;
    Task<Result<string>> AppendToList<T>(string Key, T Value, TimeSpan timeSpan = default, CancellationToken cancellationToken = default) where T: class;
    Task<Result<bool>> RemoveKey(string Key);
}