using System.Linq.Expressions;
using MongoDB.Driver;

namespace SharedKernel;


public class MongoRepository<T> : IMongoRepository<T> where T : EntityBase<Guid>{


    public readonly IMongoCollection<T> dbCollection;
    private readonly FilterDefinitionBuilder<T> filterBuilder = new();

    public MongoRepository(IMongoDatabase database, string CollectionName)
    {        
        dbCollection = database.GetCollection<T>(CollectionName);
    }

    public async Task<IReadOnlyCollection<T>> GetAll() {
        return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
    }
    
    public async Task<IReadOnlyCollection<T>> GetAll(Expression<Func<T, bool>> filter) {
        return await dbCollection.Find(filter).ToListAsync();
    }

    public async Task<T> GetLastOne(Expression<Func<T, bool>> filter) {
        return await dbCollection.Find(filter).SortByDescending(x => x.CreatedOn).Limit(1).FirstOrDefaultAsync();
    }

    public async Task<T> Get(Guid Id) {
        FilterDefinition<T> filter = filterBuilder.Eq(e => e.Id, Id);
        return await dbCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<T> Get(Expression<Func<T, bool>> filter) {
        return await dbCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<Result<string>> Create(T item) {
        try
        {
             await dbCollection.InsertOneAsync(item);
            return "";
        }
        catch (Exception ex) { return Error.New(ex.GetAllExceptions()); }
    }
    public async Task<Result<string>> CreateMany(IReadOnlyCollection<T> items) {
        try
        {
             await dbCollection.InsertManyAsync(items);
            return "";
        }
        catch (Exception ex) { return Error.New(ex.GetAllExceptions()); }
    }

    public async Task Update(T item) {
        FilterDefinition<T> filter = filterBuilder.Eq(e => e.Id, item.Id);
        await dbCollection.ReplaceOneAsync(filter, item);
    }

    public async Task Delete(Guid Id) {
        FilterDefinition<T> filter = filterBuilder.Eq(e=> e.Id, Id);
        await dbCollection.DeleteOneAsync(filter);
    }

}