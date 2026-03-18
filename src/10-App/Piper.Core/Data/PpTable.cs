using Piper.Core.Db;

namespace Piper.Core.Data;

/// <inheritdoc cref="IPpTable"/>
public class PpTable(string? name = null, ICollection<PpColumn>? columns = null) : IPpTable
{
	/// <inheritdoc/>
	public long Count { get; set; }

	/// <inheritdoc/>
	public string Name { get; } = Guard.Against.NullOrWhiteSpace(name ?? PpId.Instance.NextTable());

	/// <inheritdoc/>
	public List<PpColumn> Columns { get; set; } = (columns ?? []).ToList();

	/// <inheritdoc/>
	public async Task ClearAsync(CancellationToken ct = default)
	{
		await PpDb.Instance.CreateTableAsync(this, ct);

		Count = await CountAsync(ct);
	}

	/// <inheritdoc/>
	public async Task FetchAsync(CancellationToken ct = default)
	{
		await PpDb.Instance.FetchTableAsync(this, ct);

		await DoneAsync(ct);
	}

	/// <inheritdoc/>
	public async Task DoneAsync(CancellationToken ct = default)
	{
		Count = await CountAsync(ct);
	}

	/// <inheritdoc/>
	public Task<long> CountAsync(CancellationToken ct = default) => PpDb.Instance.CountAsync(this, ct);

	/// <inheritdoc/>
	public IAsyncEnumerable<PpRecord> QueryAllAsync(CancellationToken ct = default) => QueryAsync($"select * from {Name}", ct);

	/// <inheritdoc/>
	public IAsyncEnumerable<PpRecord> QueryAsync(string sql, CancellationToken ct = default) => PpDb.Instance.QueryAsync(this, sql, ct);

	/// <inheritdoc/>
	public Task<PpDbAppender> CreateAppenderAsync(CancellationToken ct = default) => PpDb.Instance.CreateAppenderAsync(this, ct);
}
