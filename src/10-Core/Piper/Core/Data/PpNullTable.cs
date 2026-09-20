namespace Piper.Core.Data;

/// <summary>
/// A table reference that doesn't actually do anything, but that we can use as a "NULL"-table.<br/>
/// For example, to cap off modifier stacks and plug nodes that aren't connected yet.
/// </summary>
public sealed class PpNullTable : IPpTable
{
	/// <summary>
	/// Since this table is stateless, we can use a single instance for everything.
	/// </summary>
	public static readonly IPpTable Instance = new PpNullTable();

	/// <summary>
	/// Don't allow creating multiple instances, just use the singleton instance one.
	/// </summary>
	private PpNullTable() { }

	public IEnumerable<PpColumn> Columns => [];

	public long Count { get; }

	public string Name => "_null";

	public Task<IPpTable> AddAsync(params ICollection<PpRecord> records)
	{
		return Task.FromResult<IPpTable>(this);
	}

	public Task<IPpTable> ClearAsync(CancellationToken ct = default)
	{
		return Task.FromResult<IPpTable>(this);
	}

	public Task<PpDbAppender> CreateAppenderAsync(CancellationToken ct = default)
	{
		throw new InvalidOperationException("Can't append to NULL table.");
	}

	public Task<IPpTable> DoneAsync(CancellationToken ct = default)
	{
		return Task.FromResult<IPpTable>(this);
	}

	public Task InitAsync(IEnumerable<PpColumn> columns, bool createTable = true, CancellationToken ct = default)
	{
		return Task.CompletedTask;
	}

	public Task InitFromDbAsync(CancellationToken ct = default)
	{
		return Task.CompletedTask;
	}

	public IAsyncEnumerable<PpRecord> QueryAllAsync(CancellationToken ct = default)
	{
		return AsyncEnumerable.Empty<PpRecord>();
	}

	public IAsyncEnumerable<PpRecord> QueryAsync(string sql, CancellationToken ct = default)
	{
		return AsyncEnumerable.Empty<PpRecord>();
	}
}
