using MassTransit;
namespace SharedKernel.MassTransit;

public static class MassTransitRegistration {

    public static IServiceCollection AddMassTransitWithRabbitMQ(this IServiceCollection services, IConfiguration configuration) {

        var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
        LogAndThrowException.IfNull(serviceSettings, $"ServiceSettings not found in appsettings.json");


        var rabbitMQSettings = configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();
        LogAndThrowException.IfNull(rabbitMQSettings, $"RabbitMQSettings not found in appsettings.json");

        Log.Information($"Connecting rabbitmq host: {rabbitMQSettings.Host}");
        
        services.AddMassTransit(config =>{

            config.AddConsumers(Assembly.GetEntryAssembly());

            config.UsingRabbitMq((context, cfg) =>{
                
                cfg.Host(rabbitMQSettings.Host, h =>
                {
                    h.Username( rabbitMQSettings.UserName );
                    h.Password( rabbitMQSettings.Password );
                    h.Heartbeat(TimeSpan.FromSeconds(30));
                    h.PublisherConfirmation = true;
                });

                cfg.UseMessageRetry(r => r.Interval(10, TimeSpan.FromSeconds(2)));
                cfg.UseConcurrencyLimit(16);
                cfg.UseCircuitBreaker(cb =>
                {
                    cb.TrackingPeriod = TimeSpan.FromMinutes(1);
                    cb.ActiveThreshold = 100;
                    cb.ResetInterval = TimeSpan.FromMinutes(5);
                });

                cfg.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(serviceSettings.ServiceName, false));

            });
        });

        return services;
    }
}