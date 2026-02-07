using DuckDB.NET.Data;
using DuckDB.NET.Native;
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

	// 	/// <inheritdoc/>
	// 	public async Task CreateTableAsync(PpTable table)
	// 	{
	// 		await using var db = await CreateConnectionAsync();
	// 		await using var cmd = db.CreateCommand();
	//
	// 		cmd.CommandText = $"""
	// 			DROP TABLE IF EXISTS "{table.TableName}";
	// 			CREATE TABLE "{table.TableName}"
	// 			(
	// 				data json null
	// 			)
	// 			""";
	//
	// 		await cmd.ExecuteNonQueryAsync();
	// 	}

	public async Task CreateTableAsync(PpTable table)
	{
		await using var db = await CreateConnectionAsync();

		var sb1 = new StringBuilder();
		sb1.Append(
			$"""
			DROP TABLE IF EXISTS "{table.TableName}";
			CREATE TABLE "{table.TableName}"
			(
			"""
		);

		foreach (var col in table.Columns)
		{
			sb1.Append(
				$"""
					{GetColThing(col)},

				"""
			);
		}

		sb1.Append(
			$"""
			)
			"""
		);

		await using var cmd = db.CreateCommand();

		cmd.CommandText = sb1.ToString();

		await cmd.ExecuteNonQueryAsync();
	}

	private static string GetColThing(PpColumn column)
	{
		var name = $"\"{column.Name.Replace(" ", "_")}\"";

		switch (column.PpDataType)
		{
			case PpDataType.PpBool:
				return $"{name} BOOLEAN NULL";

			case PpDataType.PpDateTime:
				return $"{name} TIMESTAMP NULL";

			case PpDataType.PpDouble:
				return $"{name} DOUBLE NULL";

			case PpDataType.PpFloat:
				return $"{name} REAL NULL";

			case PpDataType.PpGuid:
				return $"{name} UUID NULL";

			case PpDataType.PpInt32:
				return $"{name} INTEGER NULL";

			case PpDataType.PpInt64:
				return $"{name} BIGINT NULL";

			case PpDataType.PpString:
				return $"{name} TEXT NULL";

			default:
				throw new InvalidOperationException($"Unsupported column '{column.PpDataType}'");
		}
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

	// public async IAsyncEnumerable<PpRecord> QueryAsync(PpTable table, string query)
	// {
	// 	await using var db = await CreateConnectionAsync();
	// 	await using var cmd = db.CreateCommand();
	//
	// 	cmd.CommandText = query.Replace("$table", table.TableName);
	//
	// 	var reader = await cmd.ExecuteReaderAsync();
	//
	// 	while (await reader.ReadAsync())
	// 	{
	// 		var dict = new Dictionary<string, PpField>();
	//
	// 		for (var i = 0; i < reader.FieldCount; i++)
	// 		{
	// 			var name = reader.GetName(i);
	// 			var type = reader.GetFieldType(i);
	// 			var isNull = reader.IsDBNull(i);
	// 			var val = isNull ? null : reader.GetString(i);
	//
	// 			dict[name] = new(val);
	// 		}
	//
	// 		// var json = reader.GetString(0);
	// 		// yield return new PpRecord()
	// 		// {
	// 		// 	//
	// 		// 	Data = json,
	// 		// };
	// 		// var rec = PpRecord.FromJson(json);
	// 		// yield return rec;
	//
	// 		// var dbg = 2;
	//
	// 		yield return new PpRecord() { Fields = dict };
	// 	}
	// }
	//
	// public async IAsyncEnumerable<string> Query2Async(PpTable table, string query)
	// {
	// 	await using var db = await CreateConnectionAsync();
	// 	await using var cmd = db.CreateCommand();
	//
	// 	cmd.CommandText = query.Replace("$table", table.TableName);
	//
	// 	var reader = await cmd.ExecuteReaderAsync();
	//
	// 	while (await reader.ReadAsync())
	// 	{
	// 		// var dict = new Dictionary<string, PpField>();
	//
	// 		// for (var i = 0; i < reader.FieldCount; i++)
	// 		// {
	// 		// 	var name = reader.GetName(i);
	// 		// 	var type = reader.GetFieldType(i);
	// 		// 	var isNull = reader.IsDBNull(i);
	// 		// 	var val = isNull ? null : reader.GetString(i);
	// 		//
	// 		// 	dict[name] = new(val);
	// 		// }
	//
	// 		var json = reader.GetString(0);
	// 		yield return json;
	// 		// var rec = PpRecord.FromJson(json);
	// 		// yield return rec;
	//
	// 		// var dbg = 2;
	//
	// 		// yield return new PpRecord()
	// 		// {
	// 		// 	// Fields = dict,
	// 		// };
	// 	}
	// }

	public async IAsyncEnumerable<PpRecord> QueryAsync(PpTable table, string query)
	{
		_log.LogInformation("Executing query '{Query}' on table '{Table}'", query, table);

		await using var db = await CreateConnectionAsync();
		await using var cmd = db.CreateCommand();

		cmd.CommandText = query.Replace("$table", $"\"{table.TableName}\"");

		var reader = await cmd.ExecuteReaderAsync();
		while (await reader.ReadAsync())
		{
			var dict = new Dictionary<string, PpField>();

			for (var i = 0; i < reader.FieldCount; i++)
			{
				var name = reader.GetName(i);
				var type = reader.GetFieldType(i);
				// var isNull = reader.IsDBNull(i);
				var val2 = reader.GetValue(i);
				// var val = isNull ? null : reader.GetString(i);

				dict[name] = new(ToPpDataType(type), val2);
			}

			yield return new PpRecord() { Fields = dict };
		}
	}

	// csharpier-ignore-start
	private static readonly Dictionary<Type, PpDataType> _clrTypeToPpType = new()
	{
		{ typeof(bool),				PpDataType.PpBool },
		{ typeof(DateTime),			PpDataType.PpDateTime },
		{ typeof(double),			PpDataType.PpDouble },
		{ typeof(float),			PpDataType.PpFloat },
		{ typeof(Guid),				PpDataType.PpGuid },
		{ typeof(int),				PpDataType.PpInt32 },
		{ typeof(long),				PpDataType.PpInt64 },
		{ typeof(string),			PpDataType.PpString },
	};
	// csharpier-ignore-end

	private static readonly Dictionary<DuckDBType, PpDataType> _duckTypeToPpType = new()
	{
		{ DuckDBType.Boolean, PpDataType.PpBool },
		{ DuckDBType.Timestamp, PpDataType.PpDateTime },
		{ DuckDBType.Double, PpDataType.PpDouble },
		{ DuckDBType.Float, PpDataType.PpFloat },
		{ DuckDBType.Uuid, PpDataType.PpGuid },
		{ DuckDBType.Integer, PpDataType.PpInt32 },
		{ DuckDBType.BigInt, PpDataType.PpInt64 },
		{ DuckDBType.Varchar, PpDataType.PpString },
	};

	public static PpDataType ToPpDataType(Type type)
	{
		Guard.Against.Null(type);

		if (_clrTypeToPpType.TryGetValue(type, out var ppType))
		{
			return ppType;
		}

		throw new InvalidOperationException($"Cannot convert type '{type.FullName}' to {nameof(PpDataType)}.");
	}

	public static PpDataType ToPpDataType(DuckDBType type)
	{
		Guard.Against.Null(type);

		if (_duckTypeToPpType.TryGetValue(type, out var ppType))
		{
			return ppType;
		}

		throw new InvalidOperationException($"Cannot convert DuckDB type '{type}' to {nameof(PpDataType)}.");
	}
}
