namespace ClinicService;

public interface IClinicDbConnection
{
    public IDbConnection Connection { get; init; }
}
