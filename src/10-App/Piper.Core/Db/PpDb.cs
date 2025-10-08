using DuckDB.NET.Data;

namespace Piper.Core.Db;

public interface IPpDb
{
	Task<long> CountAsync(string query);

	Task CreateTableAsync(PpTable table);

	Task InsertDataAsync(string tableName, IEnumerable<PpRecord> records);

	IAsyncEnumerable<PpRecord> QueryAsync(string query);
}

public class PpDb : IPpDb
{
	public static PpDb Instance { get; } = new();

	public async Task<long> CountAsync(string query)
	{
		await using var db = await CreateConnectionAsync();
		await using var cmd = db.CreateCommand();

		cmd.CommandText = query;

		return (long)(await cmd.ExecuteScalarAsync() ?? 0);
	}

	public async Task CreateTableAsync(PpTable table)
	{
		var sb1 = new StringBuilder();
		sb1.Append(
			$"""
			DROP TABLE IF EXISTS {table.TableName};
			CREATE TABLE {table.TableName}
			(
			""");

		foreach (var col in table.Columns)
		{
			sb1.AppendLine($"{col.Name} text null,"); // TODO: Column escaping, TODO: Types
		}

		sb1.Append(
			"""
			);
			""");

		await using var db = await CreateConnectionAsync();
		await using var cmd = db.CreateCommand();

		cmd.CommandText = sb1.ToString();

		await cmd.ExecuteNonQueryAsync();
	}

	public async Task InsertDataAsync(string tableName, IEnumerable<PpRecord> records)
	{
		var db = await CreateConnectionAsync();

		using var appender = db.CreateAppender(tableName);

		foreach (var rec in records)
		{
			var row = appender.CreateRow();
			foreach (var col in rec.Fields)
			{
				var colName = col.Key;
				var colVal = col.Value?.ValueAsString;

				if (!string.IsNullOrWhiteSpace(colVal))
				{
					row.AppendValue(colVal);
				}
				else
				{
					row.AppendNullValue();
				}
			}

			row.EndRow();
		}
	}

	public async IAsyncEnumerable<PpRecord> QueryAsync(string query)
	{
		await using var db = await CreateConnectionAsync();
		await using var cmd = db.CreateCommand();

		cmd.CommandText = query;

		var reader = await cmd.ExecuteReaderAsync();
		while (await reader.ReadAsync())
		{
			var dict = new Dictionary<string, PpField>();

			for (var i = 0; i < reader.FieldCount; i++)
			{
				var name = reader.GetName(i);
				var type = reader.GetFieldType(i);
				var isNull = reader.IsDBNull(i);
				var val = isNull ? null : reader.GetString(i);

				dict[name] = new(val);
			}

			yield return new PpRecord()
			{
				Fields = dict,
			};
		}
	}

	private static async Task<DuckDBConnection> CreateConnectionAsync()
	{
		var db = new DuckDBConnection("data source=piper.db");
		await db.OpenAsync();

		return db;
	}
}