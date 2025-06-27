using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;


var builder = WebApplication.CreateBuilder(args);

try
{
    // Project Dependencies
    builder.Services.AddProjectDependencies(builder.Configuration);

    // Reverse Proxy
    builder.Services.AddReverseProxy()
        .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
        .ConfigureHttpClient((context, handler) =>
        {
            // Optimize HTTP client settings
            handler.MaxConnectionsPerServer = int.MaxValue;
            handler.PooledConnectionLifetime = TimeSpan.FromMinutes(1);
            handler.PooledConnectionIdleTimeout = TimeSpan.FromMinutes(1);
            handler.EnableMultipleHttp2Connections = true;
        })        
        ;

    
    // CORS
    // builder.Services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()));
    builder.Services.AddCors(options => 
        options.AddPolicy("AllowAll",
            policy  =>
            {
                policy.AllowAnyOrigin();
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
            })
    );


    // Rate Limiting using Fixed Window. Limiting to 10 requests and block for 10 seconds
    builder.Services.AddRateLimiter(option =>{
        option.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

        option.AddPolicy("fixed-by-ip", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 10,
                Window = TimeSpan.FromSeconds(10)
            }));
    });


    // App Metrics: Process, Runtime, AspNetCore
    builder.Services.AddOpenTelemetry()
        .WithMetrics(option => 
            option.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("GPlay.ApiGateway"))
                .AddPrometheusExporter()
                .AddAspNetCoreInstrumentation()
                .AddProcessInstrumentation()
                .AddRuntimeInstrumentation()       
        );

    // Configure Kestrel for high performance
    builder.WebHost.ConfigureKestrel(options => {

        options.Limits.MaxConcurrentConnections = int.MaxValue;
        options.Limits.MaxConcurrentUpgradedConnections = int.MaxValue;
        options.Limits.MaxRequestBodySize = null; // For scenarios with large payloads
        options.Limits.MinRequestBodyDataRate = null;
        options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(1);
        options.Limits.MinResponseDataRate = null;
        options.AddServerHeader = false;

        options.ConfigureEndpointDefaults(listenOptions =>
        {
            listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
            // listenOptions.UseHttps("https/cert.pfx", "$%09OneGames@65*GPlay#");
        });
    });

    // Configure thread pool for high throughput
    ThreadPool.SetMinThreads(200, 200);
    ThreadPool.SetMaxThreads(32767, 32767);


    // Health Check
    builder.Services.AddHealthChecks();

    builder.Services.AddOpenApi();



    var app = builder.Build();

    Console.WriteLine($"\n\nRunning Environment: {app.Environment.ApplicationName.ToUpper()}\n\n");

    if (app.Environment.IsDevelopment())
        app.MapOpenApi();
    
    // Use Cors
    app.UseCors("AllowAll");

    // app.UseHttpsRedirection();
    app.MapReverseProxy();


    // Rate Limiter
    app.UseRateLimiter();
    app.MapPrometheusScrapingEndpoint();

    // Health Check
    app.MapGroup("/api/").MapHealthChecks("health");

    app.MapGroup("/api/").AddGatewayEndpoints(builder.Configuration);

    // Security Headers
    app.UseMiddleware<SecurityHeadersMiddleware>();

    // Bad Request Logging
    app.UseStatusCodePages(context =>
    {
        if (context.HttpContext.Response.StatusCode == 400)
            Log.Warning("400 Bad Request: " + context.HttpContext.Request.Path);
        
        return Task.CompletedTask;
    });

    StartupMessage.SetStartupMessage(builder.Configuration);
    app.Run();

}
catch (Exception ex)
{
    Console.WriteLine($"API Gateway has been terminated. Reason:" + ex.GetAllExceptions());
    Log.Fatal(ex.GetAllExceptions());    
}
finally{
    Log.CloseAndFlush();
}
