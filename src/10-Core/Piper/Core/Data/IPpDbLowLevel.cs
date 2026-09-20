namespace Piper.Core.Data;

/// <summary>
/// Methods for talking to the database directly.<br/>
/// Exposed by <see cref="IPpDb"/>, to encapsulate lower-level stuff and isolate it from the higher-level ones.
/// </summary>
public interface IPpDbLowLevel
{
	/// <summary>
	/// Execute a query that doesn't return anything (though it might throw).
	/// </summary>
	Task ExecuteNonQueryAsync(string sql, CancellationToken ct = default);

	/// <summary>
	/// Execute a query that returns a list of records.
	/// </summary>
	IAsyncEnumerable<PpRecord> ExecuteQueryAsync(string query, CancellationToken ct = default);

	/// <summary>
	/// Execute a query that returns a single value (=scalar).
	/// </summary>
	Task<long> ExecuteScalarAsync(string query, CancellationToken ct = default);
}
