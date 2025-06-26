using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.OpenApi.Models;


namespace AppointmentService;

public static class CrossCuttingDependencies
{
    public static IServiceCollection AddProjectDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        
        SerilogConfiguration();

        // CORS
        services.AddCors(options => 
        options.AddPolicy("AllowAll",
            policy  =>
            {
                policy.AllowAnyOrigin();
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
            })
        );

      
        // JSON Format Config
        services.AddControllers()
        .AddJsonOptions(options => 
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = null;
            options.JsonSerializerOptions.WriteIndented = false;
        });

        // Response Compression Services
        services.AddResponseCompression(option => {
            option.EnableForHttps = true;
            option.Providers.Add<GzipCompressionProvider>();
            option.Providers.Add<BrotliCompressionProvider>();
        });

        // Configure compression options for Gzip and Brotli
        services.Configure<GzipCompressionProviderOptions>(option =>{
            option.Level = System.IO.Compression.CompressionLevel.Fastest;
        });

        services.Configure<BrotliCompressionProviderOptions>(option => {
            option.Level = System.IO.Compression.CompressionLevel.Fastest;
        });


        // Hybrid Cache
        services.AddHybridCache(options =>
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

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration["RedisSettings:Server"];
        });

        // gRPC
        services.AddGrpc(options=>
        {
            options.EnableDetailedErrors = true;
        });

        services.AddGrpcReflection();


        // Validators
        // services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
        // services.AddScoped(typeof(IPipelineBehavior<,>), typeof(PerformancePipelineBehavior<,>));


        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(),includeInternalTypes: true);
        services.AddSingleton<IHybridCacheService, HybridCacheService>();


        // MediatR
        services.AddMediatR(cfg =>{
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
            cfg.AddOpenBehavior(typeof(PerformancePipelineBehavior<,>));
        });

        // MassTransit
        // services.AddMassTransitWithRabbitMQ(configuration);

        // Redis Cache
        services.AddRedis(configuration);

        // Swagger
        services.RegisterSwagger(configuration);


        // Background Job using Quartz
        // services.AddQuartz(option =>
        // {
        //     option.UseDefaultThreadPool(tp =>
        //     {
        //         tp.MaxConcurrency = 1;
        //     });
        // });
        // services.AddQuartzHostedService();


        services.AddHybridCache();

        return services;
    }


    // Swagger
    public static IServiceCollection RegisterSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        // API Versioning
        services.AddApiVersioning(options => {

            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;

        });
        
        var xmlFile = $"AppointmentService.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        Console.WriteLine(xmlPath);
        if (!File.Exists(xmlPath))
            Console.WriteLine($"NOT FOUND ----> {xmlPath}");


        // Add Swagger
        services.AddSwaggerGen(options => {
            options.SwaggerDoc( "v1", 
            new OpenApiInfo
            {
                Title = "abs Appointment Services API",
                Version = "v1",
                Description = "abs Appointment Services API",
                TermsOfService = new Uri("https://www.linkedin.com/in/mehedisun"),
                Contact = new OpenApiContact
                {
                    Name = "Mehedi",
                    Email = "mehedi.sun@example.com",
                    Url = new Uri("https://github.com/MehediMayall")
                },
                License = new OpenApiLicense
                {
                    Name = "MIT License",
                    Url = new Uri("https://github.com/MehediMayall/AppointmentService/blob/main/LICENSE")
                },

            });

            // XML Comment Documentation

            // var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(xmlPath);

        });

        return services;
    }


    public static void SerilogConfiguration()
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .Build();


        Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProcessName()

                .Enrich.WithThreadName()
                .WriteTo.Debug()
                .WriteTo.Console()
                .Enrich.WithProperty("Environment", environment)
                .MinimumLevel.Information()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
    }
    
}