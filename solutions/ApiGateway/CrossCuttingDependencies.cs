namespace ApiGateway;

public static class CrossCuttingDependencies
{
    public static IServiceCollection AddProjectDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        // SERILOG
        Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(configuration)
        .CreateLogger();

        // JSON Format Config
        services.AddControllers()
        .AddJsonOptions(options => 
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = null;
            options.JsonSerializerOptions.WriteIndented = false;
        });

        return services;
    }

    
}