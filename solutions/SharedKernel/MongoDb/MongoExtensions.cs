using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace SharedKernel;

public static class MongoExtensions{

    public static IServiceCollection AddMongo(this IServiceCollection services, IConfiguration configuration){
        
        ServiceSettings serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();


        // MongoDb Settings
        services.AddSingleton(provider => {
            MongoDbSettings mongoDbSettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
            var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
            return mongoClient.GetDatabase(mongoDbSettings.DatabaseName);
        });

        BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
        return services;
    }


    public static IServiceCollection AddMongoRepository<T>(this IServiceCollection services, string CollectionName)
        where T : EntityBase<Guid>
    {
        
        services.AddSingleton<IMongoRepository<T>>(provider =>{
            var database = provider.GetService<IMongoDatabase>();
            return new MongoRepository<T>(database, CollectionName);
        });

        return services;
    }
}