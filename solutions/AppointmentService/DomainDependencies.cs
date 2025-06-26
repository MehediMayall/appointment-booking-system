namespace AppointmentService;

public static class DomainDependencies
{
    public static IServiceCollection AddDomainDependencies(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddScoped<UpdateAuditableEntitiesInterceptor>();

        // USED POSTGRES DATABASE
        services.AddDbContext<AppointmentDbContext>((sp, option) => {
            var auditableInterceptor = sp.GetService<UpdateAuditableEntitiesInterceptor>();

            option.AddInterceptors(auditableInterceptor);
            option.UseNpgsql(configuration["ConnectionStrings:Default"], config=>{
                config.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            })
            .UseSnakeCaseNamingConvention();
            
        });



        //Dapper
        // services.AddScoped<ICMSDbConnection>(sp =>{
        //     return  new CMSDbConnection(configuration["ConnectionStrings:CMS"]);
        // });


        // Repositories
        services.AddFeatureRepositories();


        // Unit of work
        services.AddScoped<IUnitOfWork, UnitOfWork>();


        return services;
    }

    
}