using DuckDB.NET.Data;
using Microsoft.Extensions.Logging;
using Piper.Core.Data;
using Piper.Core.Utils;

namespace Piper.Core.Db;

public interface IPpDb
{
	/// <summary>
	/// Returns the row count of the specified <paramref name="query"/>.
	/// </summary>
	Task<long> CountAsync(string query);

	/// <summary>
	/// Creates a backing table for the specified <paramref name="table"/>.
	/// </summary>
	Task CreateTableAsync(PpTable table);

	/// <summary>
	/// Execute query <paramref name="query"/> on the specified <paramref name="table"/>.
	/// </summary>
	IAsyncEnumerable<PpRecord> QueryAsync(PpTable table, string query);

	Task<PpDbAppender> CreateAppenderAsync(PpTable table);

	Task InsertDataAsync(PpTable table, IEnumerable<PpRecord> records);
}

public class PpDb : IPpDb
{
	private readonly ILogger _log = Log.For<PpDb>();

	public static PpDb Instance { get; } = new();

	private static async Task<DuckDBConnection> CreateConnectionAsync()
	{
		var db = new DuckDBConnection("data source=/home/marco/Downloads/piper.db");
		await db.OpenAsync();

		return db;
	}

	/// <inheritdoc/>
	public async Task<long> CountAsync(string query)
	{
		await using var db = await CreateConnectionAsync();
		await using var cmd = db.CreateCommand();

		cmd.CommandText = query;

		return (long)(await cmd.ExecuteScalarAsync() ?? 0);
	}

	/// <inheritdoc/>
	public async Task CreateTableAsync(PpTable table)
	{
		await using var db = await CreateConnectionAsync();
		await using var cmd = db.CreateCommand();

		cmd.CommandText = $"""
			DROP TABLE IF EXISTS "{table.TableName}";
			CREATE TABLE "{table.TableName}"
			(
				data json null
			)
			""";

		await cmd.ExecuteNonQueryAsync();
	}

	public async Task<PpDbAppender> CreateAppenderAsync(PpTable table)
	{
		var db = await CreateConnectionAsync();
		var appender = db.CreateAppender(table.TableName);

		return new PpDbAppender(db, appender, table);
	}

	public async Task InsertDataAsync(PpTable table, IEnumerable<PpRecord> records)
	{
		await using var db = await CreateConnectionAsync();

		using var appender = db.CreateAppender(table.TableName);

		foreach (var rec in records)
		{
			var row = appender.CreateRow();
			row.AppendValue(rec.AsJson());
			row.EndRow();
		}
	}

	public async IAsyncEnumerable<PpRecord> QueryAsync(PpTable table, string query)
	{
		await using var db = await CreateConnectionAsync();
		await using var cmd = db.CreateCommand();

		cmd.CommandText = query.Replace("$table", table.TableName);

		var reader = await cmd.ExecuteReaderAsync();

		while (await reader.ReadAsync())
		{
			// var dict = new Dictionary<string, PpField>();

			// for (var i = 0; i < reader.FieldCount; i++)
			// {
			// 	var name = reader.GetName(i);
			// 	var type = reader.GetFieldType(i);
			// 	var isNull = reader.IsDBNull(i);
			// 	var val = isNull ? null : reader.GetString(i);
			//
			// 	dict[name] = new(val);
			// }

			var json = reader.GetString(0);
			var rec = PpRecord.FromJson(json);
			yield return rec;

			// var dbg = 2;

			// yield return new PpRecord()
			// {
			// 	// Fields = dict,
			// };
		}
	}
}
