using System.Linq.Expressions;

namespace SharedKernel;

public interface IRepository<T> where T : class
{
    // Write
    Task Add(T entity);
    Task AddRange(List<T> entity);
    Task Update(T entity, Expression<Func<T, bool>> predicate);
    Task Delete(T entity);


    // Read
    Task<IReadOnlyCollection<T>> GetAll();
    Task<IReadOnlyCollection<T>> GetAll(Expression<Func<T,bool>> filter);
    Task<T> Get(Expression<Func<T, bool>> filter);
}