namespace ClinicService;

public static class DomainDependencies
{
    public static IServiceCollection AddDomainDependencies(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddScoped<UpdateAuditableEntitiesInterceptor>();

        // USED POSTGRES DATABASE
        services.AddDbContext<ClinicDbContext>((sp, option) => {
            var auditableInterceptor = sp.GetService<UpdateAuditableEntitiesInterceptor>();

            option.AddInterceptors(auditableInterceptor);
            option.UseNpgsql(configuration["ConnectionStrings:Default"], config=>{
                config.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            })
            .UseSnakeCaseNamingConvention();
            
        });



        //Dapper
        services.AddScoped<IClinicDbConnection>(sp =>{
            return new ClinicDbConnection(configuration["ConnectionStrings:Default"]);
        });


        // Repositories
        services.AddFeatureRepositories();


        // Unit of work
        services.AddScoped<IUnitOfWork, UnitOfWork>();


        return services;
    }

    
}