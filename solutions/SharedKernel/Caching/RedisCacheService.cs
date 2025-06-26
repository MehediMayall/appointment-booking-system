using StackExchange.Redis;
 
namespace SharedKernel;

public sealed class RedisCacheService: ICacheService
{
    private readonly IDatabase cacheDB;
    private readonly RedisSettings redisSettings;
    private TimeSpan cacheExpiryTime = TimeSpan.FromMinutes(30);

    public RedisCacheService(IConfiguration configuration, IConnectionMultiplexer connectionMultiplexer)
    {        
        cacheDB = connectionMultiplexer.GetDatabase();
        this.redisSettings = configuration.GetSection(nameof(RedisSettings)).Get<RedisSettings>();


        // Set Default Expiry
        if (redisSettings.DefaultExpirationInMinutes > 0) 
            ValidateAndSetCacheExpiryTime(TimeSpan.FromMinutes(redisSettings.DefaultExpirationInMinutes));
    }

    public IDatabase GetDatabase() => cacheDB;

    public async Task<Result<T?>> GetAsync<T>(string Key, CancellationToken cancellationToken = default) where T: class
    {
        if (string.IsNullOrEmpty(Key)) 
            return  RedisCacheErrors.EmptyArgument("Key is null or empty in RedisCacheService.GetAsync");

        var cachedValue = await cacheDB.StringGetAsync(Key);
        
        if (string.IsNullOrEmpty(cachedValue)) 
            return RedisCacheErrors.EmptyContent(Key);
                
        return JsonSerializer.Deserialize<T>(cachedValue);
    }

    public async Task<Result<List<T?>>> GetListAsync<T>(string Key, CancellationToken cancellationToken = default) where T: class
    {
        if (string.IsNullOrEmpty(Key)) 
            return  RedisCacheErrors.EmptyArgument("Key is null or empty in RedisCacheService.GetAsync");

        
                
        var cachedValue = await cacheDB.ListRangeAsync(Key);
        return cachedValue.Select(x => JsonSerializer.Deserialize<T>(x)).ToList(); // JsonSerializer.Serialize<T>();
    }

    public async Task<Result<T?>> GetOrCreateAsync<T>(string Key, Func<Task<T>> NonCached) where T: class => await GetOrCreateAsync<T>(Key, NonCached, default);
    public async Task<Result<T?>> GetOrCreateAsync<T>(string Key, Func<Task<T>> NonCached, TimeSpan timeSpan = default, CancellationToken cancellationToken = default) where T: class
    {
        if (string.IsNullOrEmpty(Key)) return  Error.New($"Key is null or empty  in RedisCacheService.SetAsync");
        ValidateAndSetCacheExpiryTime(timeSpan);


        var cachedValue = await cacheDB.StringGetAsync(Key);
        if (!string.IsNullOrEmpty(cachedValue))
            return JsonSerializer.Deserialize<T>(cachedValue);

        var nonCachedValue = await NonCached();
        await SetAsync<T>(Key, nonCachedValue, cacheExpiryTime);
        return nonCachedValue;
    }
    public async Task<Result<string>> AppendToList<T>(string Key, T Value, TimeSpan timeSpan = default, CancellationToken cancellationToken = default) where T: class
    {
        if (string.IsNullOrEmpty(Key)) 
            return  Error.New($"Key is null or empty  in RedisCacheService.SetAsync");
        
        ValidateAndSetCacheExpiryTime(timeSpan);

        await cacheDB.ListRightPushAsync(Key, JsonSerializer.Serialize(Value));
            
        return "";
    }
    public async Task<Result<bool>> RemoveKey(string Key) =>  
        await cacheDB.KeyDeleteAsync(Key);



    public async Task<Result<bool>> SetAsync<T>(string Key, T Value) where T: class => await SetAsync<T>(Key, Value, default);
     
    public async Task<Result<bool>> SetAsync<T>(string Key, T Value, TimeSpan timeSpan,  CancellationToken cancellationToken = default) where T: class
    {
        ValidateAndSetCacheExpiryTime(timeSpan);

        if (string.IsNullOrEmpty(Key)) return  Error.New($"Key is null or empty in RedisCacheService.SetAsync.");
        return await cacheDB.StringSetAsync(Key, JsonSerializer.Serialize(Value), cacheExpiryTime);
    }

    private void ValidateAndSetCacheExpiryTime(TimeSpan timeSpan)
    {
        if (timeSpan == default)
            cacheExpiryTime = TimeSpan.FromMinutes(30);
        else
            cacheExpiryTime = timeSpan;
    }


    public async Task<Result<IReadOnlyCollection<T>>> GetAllHashAsync<T>(string Key) where T: class
    {
        var hashEntries = await cacheDB.HashGetAllAsync(Key);
        var result = hashEntries.Select(entry => 
            JsonSerializer.Deserialize<T>(entry.Value)
        ).ToList();

        if (result.Count == 0) 
            return RedisCacheErrors.EmptyContent(Key);
        
        return result;
    }



}