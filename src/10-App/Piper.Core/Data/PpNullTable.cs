namespace Piper.Core.Data;

public class PpNullTable : IPpTable
{
	public static readonly IPpTable Instance = new PpNullTable();

	private PpNullTable() { }

	public List<PpColumn> Columns
	{
		get => [];
		set => throw new InvalidOperationException("Can't change NULL table.");
	}

	public long Count { get; set; }

	public string Name => "_null";

	public Task AddRangeAsync(IEnumerable<PpRecord> records, CancellationToken ct = default) => Task.CompletedTask;

	public Task<IPpTable> AddAsync(params IEnumerable<PpRecord> records) => Task.FromResult<IPpTable>(this);

	public IPpTable Clear() => this;

	public Task<IPpTable> ClearAsync(CancellationToken ct = default) => Task.FromResult<IPpTable>(this);

	public Task FetchAsync(CancellationToken ct = default) => Task.CompletedTask;

	public Task<long> CountAsync(CancellationToken ct = default) => Task.FromResult(0L);

	public Task<IPpTable> DoneAsync(CancellationToken ct = default) => Task.FromResult<IPpTable>(this);

	public void Init(IEnumerable<PpColumn> columns, bool createTable = true) { }

	public void Init(PpRecord record, bool createTable = true) { }

	public IAsyncEnumerable<PpRecord> QueryAllAsync(CancellationToken ct = default) => AsyncEnumerable.Empty<PpRecord>();

	public IAsyncEnumerable<PpRecord> QueryAsync(string sql, CancellationToken ct = default) => AsyncEnumerable.Empty<PpRecord>();

	public Task<PpDbAppender> CreateAppenderAsync(CancellationToken ct = default) =>
		throw new InvalidOperationException("Can't append to NULL table.");

	public IPpTable WithColumn(PpDataType type, string name)
	{
		throw new NotImplementedException();
	}

	public IPpTable WithColumns()
	{
		throw new NotImplementedException();
	}

	public IPpTable WithRecord(params PpRecord[] record)
	{
		throw new NotImplementedException();
	}

	public IPpTable WithRecords()
	{
		throw new NotImplementedException();
	}
}
