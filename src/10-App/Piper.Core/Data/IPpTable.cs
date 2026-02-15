namespace Piper.Core.Data;

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
