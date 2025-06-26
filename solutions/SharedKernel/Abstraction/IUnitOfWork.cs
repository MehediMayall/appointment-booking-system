namespace SharedKernel;

public interface IUnitOfWork
{
    Task<Result<string>> SaveChangesAsync(CancellationToken cancellationToken = default);
}