using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Caching.Hybrid;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

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

    // Grpc
    builder.Services.AddGrpc(options=>
    {
        options.EnableDetailedErrors = true;
    });

    builder.Services.AddGrpcReflection();

    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ConfigureEndpointDefaults(listenOptions =>
        {
            listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
            // listenOptions.UseHttps("https/cert.pfx", "$%09OneGames@65*abs#");
        });
    });

    // CORS
    builder.Services.AddCors(options => 
    options.AddPolicy("AllowAll",
        policy  =>
        {
            policy.AllowAnyOrigin();
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
        })
    );



    var serviceName = "ClinicService";
 
    
    // Hybrid Cache
    builder.Services.AddHybridCache(options =>
    {
        // Maximum size of cached items
        options.MaximumPayloadBytes = 1024 * 1024 * 10; // 10MB
        options.MaximumKeyLength = 512;

        // Default timeouts
        options.DefaultEntryOptions = new HybridCacheEntryOptions
        {
            Expiration = TimeSpan.FromMinutes(600),
            LocalCacheExpiration = TimeSpan.FromMinutes(600)
        };
    });

    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = builder.Configuration["RedisSettings:Server"];
    });


    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {

        app.MapOpenApi();
        // Swagger
        app.UseSwagger();
        app.UseSwaggerUI(options => {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            options.RoutePrefix = string.Empty;
        });
    } else {
        // Swagger
        app.UseSwagger();
        app.UseSwaggerUI(options => {
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