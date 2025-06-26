using Microsoft.AspNetCore.Server.Kestrel.Core;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();

try
{
    // App Settings
    builder.Services.ConfigureCacheAppSettings(builder.Configuration); 

    // Cross Cutting Dependencies
    builder.Services.AddProjectDependencies(builder.Configuration);

    // Domain Dependencies
    builder.Services.AddDomainDependencies(builder.Configuration);

    // Health Check
    builder.Services.AddHealthChecks();

    // Global Exception Logger
    builder.Services.AddSingleton<GlobalExceptionsLogger>();
    builder.Services.AddSingleton<IdempotencyMiddleware>();

    builder.Services.AddHttpContextAccessor();
    builder.Services.AddDataProtection();

    builder.Host.UseSerilog();




    // Web Server Configuration
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ConfigureEndpointDefaults(listenOptions =>
        {
            listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
        });
    });


    var app = builder.Build();

    // Swagger
    if (app.Environment.IsDevelopment())
    {

        app.MapOpenApi();
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            options.RoutePrefix = string.Empty;
        });
    }
    else
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/api/auth/swagger/v1/swagger.json", "v1");
            options.RoutePrefix = string.Empty;
        });
    }

    // Authorization
    app.UseAuthorization();

    // Use Cors
    app.UseCors("AllowAll");    

    // Global Exception Logger
    app.UseMiddleware<GlobalExceptionsLogger>();

    // Idempotency Middleware
    app.UseMiddleware<IdempotencyMiddleware>();


    // Use Compression
    app.UseResponseCompression();

    // HTTPS
    // app.UseHttpsRedirection();

    // Common Endpoints
    app.AddCommonEndpoints(builder.Configuration);


    // Health Check
    app.MapHealthChecks("/health");

    // Startup Message    
    StartupMessage.SetStartupMessage(builder.Configuration);
    app.Run();

}
catch (Exception ex)
{
    Console.WriteLine($"Clinic Service has been terminated. Reason:" + ex.GetAllExceptions());
    Log.Fatal(ex.GetAllExceptions());    
}
finally{
    Log.CloseAndFlush();
}


public partial class Program {}