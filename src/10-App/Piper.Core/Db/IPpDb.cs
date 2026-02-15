using Piper.Core.Data;

namespace Piper.Core.Db;

public interface IPpDb
{
	/// <summary>
	/// Provides direct access to the underlying DuckDB instance.
	/// </summary>
	IPpDbLowLevel LowLevel { get; }

	/// <summary>
	/// Creates a table in the database, based on the structure as describe by the specified <paramref name="table"/>.<br/>
	/// So PpTable -> DuckDB.
	/// </summary>
	Task CreateTableAsync(IPpTable table, CancellationToken ct = default);

	/// <summary>
	/// Loads a structure from the database, and stores it in the specified <paramref name="table"/>.<br/>
	/// So DuckDB -> PpTable.
	/// </summary>
	Task FetchTableAsync(IPpTable table, CancellationToken ct = default);

	/// <summary>
	/// TODO
	/// </summary>
	Task<long> CountAsync(IPpTable table, CancellationToken ct = default);

	/// <summary>
	/// Executes a query on the specified <paramref name="table"/>.
	/// The table will be available as a query source, using the variable '$table'.
	/// </summary>
	IAsyncEnumerable<PpRecord> QueryAsync(IPpTable table, string query, CancellationToken ct = default);

	/// <summary>
	/// Executes a query on the specified <paramref name="tables"/>.<br/>
	/// Each table will be available as a query source, using the variable '$table0', '$table1', etc.
	/// </summary>
	IAsyncEnumerable<PpRecord> QueryAsync(IList<IPpTable> tables, string query, CancellationToken ct = default);

	/// <summary>
	/// Creates an appender, that can be used to efficiently bulk insert into the specified <paramref name="table"/>.
	/// </summary>
	Task<PpDbAppender> CreateAppenderAsync(IPpTable table, CancellationToken ct = default);
}
