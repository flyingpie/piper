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

	Task<IPpTable> AddAsync(params IEnumerable<PpRecord> records);

	IPpTable Clear();

	Task<IPpTable> ClearAsync(CancellationToken ct = default);

	Task FetchAsync(CancellationToken ct = default);

	// Task<long> CountAsync(CancellationToken ct = default);

	Task<IPpTable> DoneAsync(CancellationToken ct = default);

	void Init(IEnumerable<PpColumn> columns, bool createTable = true);

	void Init(PpRecord record, bool createTable = true);

	IAsyncEnumerable<PpRecord> QueryAllAsync(CancellationToken ct = default);

	IAsyncEnumerable<PpRecord> QueryAsync(string sql, CancellationToken ct = default);

	Task<PpDbAppender> CreateAppenderAsync(CancellationToken ct = default);
}
