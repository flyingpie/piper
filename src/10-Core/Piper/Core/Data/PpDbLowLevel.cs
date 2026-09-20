using System.Runtime.CompilerServices;
using DuckDB.NET.Data;

namespace Piper.Core.Data;

public class PpDbLowLevel(Func<Task<DuckDBCommand>> cmdFactory) : IPpDbLowLevel
{
	private readonly Func<Task<DuckDBCommand>> _cmdFactory = Guard.Against.Null(cmdFactory);

	public async Task ExecuteNonQueryAsync(string sql, CancellationToken ct = default)
	{
		await using var cmd = await _cmdFactory();

		cmd.CommandText = sql;

		await cmd.ExecuteNonQueryAsync(ct);
	}

	public async IAsyncEnumerable<PpRecord> ExecuteQueryAsync(string query, [EnumeratorCancellation] CancellationToken ct = default)
	{
		await using var cmd = await _cmdFactory();

		cmd.CommandText = query;

		var reader = await cmd.ExecuteReaderAsync(ct);
		while (await reader.ReadAsync(ct))
		{
			var dict = new Dictionary<string, PpField>();

			for (var i = 0; i < reader.FieldCount; i++)
			{
				var name = reader.GetName(i);
				var type = reader.GetFieldType(i);
				var val2 = reader.GetValue(i);

				dict[name] = new(type.ToPpDataType(), val2);
			}

			yield return new PpRecord() { Fields = dict };
		}
	}

	public async Task<long> ExecuteScalarAsync(string query, CancellationToken ct = default)
	{
		Guard.Against.NullOrWhiteSpace(query);

		await using var cmd = await _cmdFactory();

		cmd.CommandText = query;

		return (long)(await cmd.ExecuteScalarAsync(ct) ?? 0);
	}
}
