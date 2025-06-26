namespace SharedKernel;

public static class AppSettingsConfiguration
{
    
    public static void ConfigureOrThrowError<T>(IServiceCollection services, IConfiguration configuration) where T : class
    {
        string serviceName = typeof(T).Name.Split(".").LastOrDefault();

        var sections = configuration.GetSection(serviceName);
        LogAndThrowException.IfNull(sections, $"Configuration loading failed in ConfigureOrThrowError. Type {serviceName} not found in appsettings.json.");
        
        var settings = sections.Get<T>();
        LogAndThrowException.IfNull(settings, $"Configuration loading failed in ConfigureOrThrowError. Type {serviceName} not found in appsettings.json.");      

        services.Configure<T>(sections); 
        Log.Information($"{serviceName} - load SUCCESS");
    }
}