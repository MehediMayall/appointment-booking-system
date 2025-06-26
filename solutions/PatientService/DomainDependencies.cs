namespace PatientService;

public static class DomainDependencies
{
    public static IServiceCollection AddDomainDependencies(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddMongo(configuration);
        services.AddMongoRepository<Patient>("patients");



        // Repositories
        services.AddFeatureRepositories();



        return services;
    }

    
}