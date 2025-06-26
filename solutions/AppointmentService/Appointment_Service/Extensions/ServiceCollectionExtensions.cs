namespace AppointmentService;

public static class ServiceCollectionExtensions
{

    public static IServiceCollection AddFeatureRepositories(this IServiceCollection services)
    {
        var assembly = typeof(Program).Assembly;

        var repositoryInterfaces = assembly.GetTypes()
            .Where(t => t.IsInterface && t.Name.EndsWith("Repository"));

        foreach (var repositoryInterface in repositoryInterfaces)
        {
            var repositoryImplementation = assembly.GetTypes()
                .Single(t => 
                    t.IsClass && 
                    !t.IsAbstract && 
                    t.Name.EndsWith("Repository") &&
                    repositoryInterface.IsAssignableFrom(t)
                );

            if (repositoryImplementation != null)
                services.AddScoped(repositoryInterface, repositoryImplementation);
            
            
        }

        return services;
    }


    public static IServiceCollection AddFeatureServices(this IServiceCollection services)
    {
        var assembly = typeof(Program).Assembly;

        var repositoryInterfaces = assembly.GetTypes()
            .Where(t => t.IsInterface && t.Name.EndsWith("Service"));

        foreach (var repositoryInterface in repositoryInterfaces)
        {
            var repositoryImplementation = assembly.GetTypes()
                .Single(t => 
                    t.IsClass && 
                    !t.IsAbstract && 
                    t.Name.EndsWith("Service") &&
                    repositoryInterface.IsAssignableFrom(t)
                );

            if (repositoryImplementation != null)
                services.AddScoped(repositoryInterface, repositoryImplementation);
        }

        return services;
    }
}