using Microsoft.EntityFrameworkCore;

namespace SharedKernel;

public static class PostgresExtension{
    public static IServiceCollection AddPostgres<T>(this IServiceCollection services, IConfiguration configuration)
        where T : DbContext
    {

        var connectionString = configuration.GetConnectionString("Default");
        LogAndThrowException.IfNullOrEmpty(connectionString, $"Postgres database connectionstring not found in appsettings.json");

        services.AddDbContext<T>(options => options.UseNpgsql(connectionString), ServiceLifetime.Scoped);
        return services;
    }
}