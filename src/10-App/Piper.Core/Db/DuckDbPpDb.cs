using Dapper;
using DuckDB.NET.Data;
using System.Text;

namespace Piper.Core.Db;

public class DuckDbPpDb : IPpDb
{
	public static DuckDbPpDb Instance { get; } = new();

	private DuckDBConnection? _conn;

	private async Task<DuckDBConnection> CreateConnectionAsync()
	{
		if (_conn == null)
		{
			_conn = new DuckDBConnection("data source=piper.db");
			await _conn.OpenAsync();
		}

		return _conn;
	}

	public async Task LoadAsync(PpDataFrame frame)
	{
		await using var db = await CreateConnectionAsync();

		var cols = frame.FieldNames.ToList();

		await CreateTableAsync(db, cols);
		InsertDataAsync(db, frame);
	}

	public async Task<PpDataFrame> QueryAsync(string sql)
	{
		await using var db = await CreateConnectionAsync();

		return await ReadDataAsync(db, sql);
	}

	private static async Task CreateTableAsync(DuckDBConnection db, List<string> cols)
	{
		var sb1 = new StringBuilder();
		sb1.Append(
			"""
			DROP TABLE IF EXISTS t1;
			CREATE TABLE t1
			(
			""");

		foreach (var col in cols)
		{
			sb1.AppendLine($"{col} text null,"); // TODO: Column escaping, TODO: Types
		}

		sb1.Append(
			"""
			);
			""");

		await db.ExecuteAsync(sb1.ToString());
	}

	private static void InsertDataAsync(DuckDBConnection db, PpDataFrame frame)
	{
		using var appender = db.CreateAppender("t1");

		foreach (var rec in frame.Records)
		{
			var row = appender.CreateRow();
			foreach (var col in frame.FieldNames)
			{
				var v = rec.Fields.TryGetValue(col, out var val) ? val?.ValueAsString : null;

				if (!string.IsNullOrWhiteSpace(v))
				{
					row.AppendValue(v);
				}
				else
				{
					row.AppendNullValue();
				}
			}

			row.EndRow();
		}
	}

	private static async Task<PpDataFrame> ReadDataAsync(DuckDBConnection db, string query)
	{
		using var cmd = db.CreateCommand();
		// cmd.CommandText =
		// 	"""
		// 	select * from t1 limit 250
		// 	""";
		cmd.CommandText = query;

		var outLines = new PpDataFrame();

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

			outLines.Records.Add(new()
			{
				Fields = dict,
			});
		}

		return outLines;
	}

	public async Task CreateTableAsync(string tableName, List<PpColumn> cols)
	{
		var db = await CreateConnectionAsync();

		var sb1 = new StringBuilder();
		sb1.Append(
			$"""
			DROP TABLE IF EXISTS {tableName};
			CREATE TABLE {tableName}
			(
			""");

		foreach (var col in cols)
		{
			sb1.AppendLine($"{col.Name} text null,"); // TODO: Column escaping, TODO: Types
		}

		sb1.Append(
			"""
			);
			""");

		await db.ExecuteAsync(sb1.ToString());
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
				// var v = rec.Fields.TryGetValue(col, out var val) ? val?.ValueAsString : null;
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
}