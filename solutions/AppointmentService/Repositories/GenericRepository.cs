using System.Linq.Expressions;

namespace AppointmentService;

public class GenericRepository<T>: IRepository<T> where T: class
{
    public readonly DbContext _context;
    private DbSet<T> table;
    public GenericRepository(DbContext dbContext)
    {
        _context = dbContext;
        table = _context.Set<T>();
    }
    
    

    public async Task<T> Get(Expression<Func<T, bool>> filter)
    {
        return await _context.Set<T>().FirstOrDefaultAsync(filter);
    }

    public async Task<IReadOnlyCollection<T>> GetAll()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<IReadOnlyCollection<T>> GetAll(Expression<Func<T, bool>> filter)
    {
        return await _context.Set<T>().Where(filter).ToListAsync();
    }

    
    public async Task Add(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
    }

    public async Task Update(T entity, Expression<Func<T, bool>> predicate)
    {
        T value = await table.FirstOrDefaultAsync(predicate);
        if(value is not null)
        {
            _context.Entry(value).CurrentValues.SetValues(entity);
        }        
    }

    public async Task Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public async Task AddRange(List<T> entity)
    {
        await _context.Set<T>().AddRangeAsync(entity);
    }
}