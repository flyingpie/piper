namespace Piper.Core.Data;

/// <summary>
/// A table forms the basic data container that operators like nodes work on.<br/>
/// It's technically just a pointer to a table (or view) in DuckDB.
/// </summary>
public interface IPpTable
{
	IEnumerable<PpColumn> Columns { get; }

	long Count { get; }

	string Name { get; }

	Task<IPpTable> AddAsync(params ICollection<PpRecord> records);

	IPpTable Clear();

	Task<PpDbAppender> CreateAppenderAsync(CancellationToken ct = default);

	Task<IPpTable> DoneAsync(CancellationToken ct = default);

	Task InitAsync(IEnumerable<PpColumn> columns, bool createTable = true, CancellationToken ct = default);

	Task InitFromDbAsync(CancellationToken ct = default);

	IAsyncEnumerable<PpRecord> QueryAllAsync(CancellationToken ct = default);

	IAsyncEnumerable<PpRecord> QueryAsync(string sql, CancellationToken ct = default);
}
