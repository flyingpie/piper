using Piper.Core.Db;
using shortid;

namespace Piper.Core.Data;

public class PpId
{
	private int _i;
	private int _j;

	public static PpId Instance { get; } = new();

	// public string Next() => $"{(char)_i++}{_j++}";
	public string Next() => $"t{Guid.CreateVersion7().ToString().ToLowerInvariant().Replace("-", "")}";
}

/// <summary>
/// A table forms the basic data container that operators like nodes work on.<br/>
/// It's technically just a pointer to a table in DuckDB.
/// </summary>
public class PpTable(string? name = null, ICollection<PpColumn>? columns = null) : IPpTable
{
	/// <inheritdoc/>
	public long Count { get; set; }

	/// <inheritdoc/>
	public string Name { get; } = Guard.Against.NullOrWhiteSpace(name ?? PpId.Instance.Next());

	/// <inheritdoc/>
	public List<PpColumn> Columns { get; set; } = (columns ?? []).ToList();

	// public static string GetTableName(PpNode node, string propName)
	// {
	// 	return $"{node.GetType().Name}_{node.NodeId}_{propName}";
	// }

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
