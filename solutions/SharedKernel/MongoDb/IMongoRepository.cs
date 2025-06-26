using System.Linq.Expressions;

namespace SharedKernel;

public interface IMongoRepository<T> where T : EntityBase<Guid>
{
    Task<IReadOnlyCollection<T>> GetAll();
    Task<IReadOnlyCollection<T>> GetAll(Expression<Func<T,bool>> filter);
    Task<T> GetLastOne(Expression<Func<T, bool>> filter);
    Task<T> Get(Guid id);
    Task<T> Get(Expression<Func<T, bool>> filter);
    Task<Result<string>> Create(T item);
    Task Update(T item);
    Task Delete(Guid id);
}