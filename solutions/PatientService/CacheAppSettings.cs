namespace PatientService;

public static class CacheAppSettings
{
    public static void ConfigureCacheAppSettings(this IServiceCollection services, IConfiguration configuration){

        // JWT
        // AppSettingsConfiguration.ConfigureOrThrowError<JWTSettings>(services, configuration);

        // // RABBIT MQ
        // AppSettingsConfiguration.ConfigureOrThrowError<RabbitMQSettings>(services, configuration);

        // // Service
        // AppSettingsConfiguration.ConfigureOrThrowError<ServiceSettings>(services, configuration);

        // // Redis
        // AppSettingsConfiguration.ConfigureOrThrowError<RedisSettings>(services, configuration);

    }

     
}