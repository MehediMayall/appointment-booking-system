using StackExchange.Redis;

namespace SharedKernel;

public static class RedisExtension
{
    public static void AddRedis(this IServiceCollection services, IConfiguration configuration){
        var redisSettings = configuration.GetSection(nameof(RedisSettings)).Get<RedisSettings>();

        LogAndThrowException.IfNull(redisSettings, $"Trying to load RedisSettings Configuration but not found in appsettings.json");

        var redisConnection = ConnectionMultiplexer.Connect(redisSettings.Server);
        services.AddSingleton<IConnectionMultiplexer>(redisConnection);

        Log.Information("REDIS CONNECTED SUCCESSFULLY");
    }
}