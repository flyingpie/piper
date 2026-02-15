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

	public Task ClearAsync(CancellationToken ct = default) => Task.CompletedTask;

	public Task FetchAsync(CancellationToken ct = default) => Task.CompletedTask;

	public Task<long> CountAsync(CancellationToken ct = default) => Task.FromResult(0L);

	public Task DoneAsync(CancellationToken ct = default) => Task.CompletedTask;

	public IAsyncEnumerable<PpRecord> QueryAllAsync(CancellationToken ct = default) => AsyncEnumerable.Empty<PpRecord>();

	public IAsyncEnumerable<PpRecord> QueryAsync(string sql, CancellationToken ct = default) => AsyncEnumerable.Empty<PpRecord>();

	public Task<PpDbAppender> CreateAppenderAsync(CancellationToken ct = default) =>
		throw new InvalidOperationException("Can't append to NULL table.");
}
