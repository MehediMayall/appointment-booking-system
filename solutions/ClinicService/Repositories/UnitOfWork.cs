
using Npgsql;

namespace ClinicService;

public sealed class UnitOfWork( ClinicDbContext _ClinicDBContext) : IUnitOfWork
{     
    public async Task<Result<string>> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _ClinicDBContext.SaveChangesAsync();

            return "Saved successfully";
        }
        // Postgres Sql Exception
        catch(DbUpdateException ex) when (ex.InnerException is PostgresException pgEx) {
            var errorCode = Enum.GetName(typeof(PostgresErrorCode), int.Parse(pgEx.SqlState));
            return Error.New(errorCode, ex.GetAllExceptions());
        }
        catch(Exception ex) {
            return Error.New(ex.GetAllExceptions());
        }   
    }
}