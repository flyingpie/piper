using Piper.Core.Data;

namespace Piper.Core.Db;

public interface IPpDbLowLevel
{
	Task ExecuteNonQueryAsync(string sql, CancellationToken ct = default);

	IAsyncEnumerable<PpRecord> ExecuteQueryAsync(string query, CancellationToken ct = default);

	Task<long> ExecuteScalarAsync(string query, CancellationToken ct = default);
}
