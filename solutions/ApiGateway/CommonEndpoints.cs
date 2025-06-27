namespace ApiGateway;

public static class CommonEndpoints{

    public static void AddGatewayEndpoints(this RouteGroupBuilder routeGroup, IConfiguration configuration) {

        ServiceSettings serviceSettings = configuration.GetSection("ServiceSettings").Get<ServiceSettings>();
        if (serviceSettings == null) 
        {
            Log.Fatal($"ServiceSettings not found in appsettings.json");
            throw new ArgumentNullException($"ServiceSettings not found in appsettings.json");
        } 

        string welcome = $"Welcome to {serviceSettings.ServiceName} Service. Version: {serviceSettings.ServiceVersion}. Environment: {serviceSettings.ServiceEnvironment}. Current System date & time:{DateTime.Now}. Machine Name:  {Environment.MachineName}";


        routeGroup.MapGet("welcome", async() => { return Results.Ok(welcome); });
        routeGroup.MapGet("", async() => { return Results.Ok(welcome); });


        // Serilog : Temprorary
        routeGroup.MapGet("/logs", async(IWebHostEnvironment env)=>{

            string path = Path.Combine(env.ContentRootPath,"/log/gateway.log.txt");
            // Log.Error($"An error occured in Gateway. -> {path} at {DateTime.Now}"); 
            // Log.Error($"Reading logs from {path} at {DateTime.Now}");
    
            if (!File.Exists(path))
                return Results.NoContent();


            await Log.CloseAndFlushAsync();
            string logs = await File.ReadAllTextAsync(path);
            
            Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
            return Results.Ok(logs);
            // return Results.Ok(path);
        });
    }
}