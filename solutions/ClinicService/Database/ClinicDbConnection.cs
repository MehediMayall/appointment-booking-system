namespace ClinicService;

public sealed class ClinicDbConnection : IClinicDbConnection
{
    public IDbConnection Connection { get; init; }

    public ClinicDbConnection(string ConnectionStrings)
    {
        Connection = new NpgsqlConnection(ConnectionStrings);
    }
}