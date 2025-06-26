using System.Diagnostics;

namespace ClinicService;

public static class SlowQueriesLoggingExtensions
{
    public static async Task<IEnumerable<T>> QueryWithLoggingAsync<T>(
        this IDbConnection connection,
        string sql,
        object? param = null,
        int slowQueryThresholdMs = 500,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null)
    {
        var stopwatch = Stopwatch.StartNew();
        var result = await connection.QueryAsync<T>(sql, param, transaction, commandTimeout, commandType);
        stopwatch.Stop();

        if (stopwatch.ElapsedMilliseconds > slowQueryThresholdMs)
        {
            Log.Warning("Slow Dapper query: {ElapsedMs} ms\nQuery: {Sql}\nParams: {@Params}",
                stopwatch.ElapsedMilliseconds, sql, param);
        }

        return result;
    }
    public static async Task<T> QueryFirstWithLoggingAsync<T>(
        this IDbConnection connection,
        string sql,
        object? param,
        int slowQueryThresholdMs = 500,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null)
    {
        var stopwatch = Stopwatch.StartNew();
        var result = await connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction, commandTimeout, commandType);
        stopwatch.Stop();

        if (stopwatch.ElapsedMilliseconds > slowQueryThresholdMs)
        {
            Log.Warning("Slow Dapper query: {ElapsedMs} ms\nQuery: {Sql}\nParams: {@Params}",
                stopwatch.ElapsedMilliseconds, sql, param);
        }

        return result;
    }



}