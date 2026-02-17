namespace Piper.Core.Data;

/// <summary>
/// A table forms the basic data container that operators like nodes work on.<br/>
/// It's technically just a pointer to a table (or view) in DuckDB.
/// </summary>
public interface IPpTable
{
	List<PpColumn> Columns { get; set; }

	long Count { get; set; }

	string Name { get; }

	Task ClearAsync(CancellationToken ct = default);

	Task FetchAsync(CancellationToken ct = default);

	Task<long> CountAsync(CancellationToken ct = default);

	Task DoneAsync(CancellationToken ct = default);

	IAsyncEnumerable<PpRecord> QueryAllAsync(CancellationToken ct = default);

	IAsyncEnumerable<PpRecord> QueryAsync(string sql, CancellationToken ct = default);

	Task<PpDbAppender> CreateAppenderAsync(CancellationToken ct = default);
}
