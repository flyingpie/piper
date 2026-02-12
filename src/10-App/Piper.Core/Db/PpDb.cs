using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;
using Dapper;
using DuckDB.NET.Data;
using DuckDB.NET.Native;
using Microsoft.Extensions.Logging;
using Piper.Core.Data;
using Piper.Core.Utils;

namespace Piper.Core.Db;

public interface IPpDb
{
	// IReadOnlyCollection<PpTable> Tables { get; }

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

	IAsyncEnumerable<PpRecord> QueryAsync(IList<PpTable> tables, string query);

	Task<PpDbAppender> CreateAppenderAsync(PpTable table);

	// Task InsertDataAsync(PpTable table, IEnumerable<PpRecord> records);

	// Task<ICollection<PpTable>> ListTablesAsync(CancellationToken ct = default);
	Task ExecuteNonQueryAsync(string sql);

	Task InitTableAsync(PpTable table);

	IAsyncEnumerable<PpRecord> RawQueryAsync(string query);
}

public class PpDb : IPpDb
{
	private readonly ILogger _log = Log.For<PpDb>();
	private readonly string _path;

	private readonly DuckDBConnection _conn;
	private bool _isOpen;
	private readonly SemaphoreSlim _lock = new(1);

	public PpDb(string path = "/home/marco/Downloads/piper.db")
	{
		_path = Guard.Against.NullOrWhiteSpace(path);
		// _conn = new DuckDBConnection($"DataSource=:memory:?cache=shared");
		_conn = new DuckDBConnection($"DataSource={path}");
	}

	public static PpDb Instance { get; } = new();

	// public IReadOnlyCollection<PpTable> Tables { get; private set; }

	private async Task<DuckDBConnection> CreateConnectionAsync2()
	{
		// var db = new DuckDBConnection($"data source={_path}");
		// var db = new DuckDBConnection($"DataSource=:memory:?cache=shared");
		if (!_isOpen)
		{
			await _conn.OpenAsync();
			_isOpen = true;
		}

		return _conn;
	}

	public async Task OpenAsync()
	{
		if (_isOpen)
		{
			return;
		}

		await _lock.WaitAsync();

		try
		{
			if (_isOpen)
			{
				return;
			}

			await _conn.OpenAsync();
			_isOpen = true;
		}
		finally
		{
			_lock.Release();
		}
	}

	public async Task<DuckDBCommand> CreateCommandAsync()
	{
		await OpenAsync();

		return _conn.CreateCommand();
	}

	/// <inheritdoc/>
	public async Task<long> CountAsync(string query)
	{
		// var db = await CreateConnectionAsync();
		await using var cmd = await CreateCommandAsync();

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
		// var db = await CreateConnectionAsync();

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

		// await using var cmd = db.CreateCommand();
		await using var cmd = await CreateCommandAsync();

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

			// case PpDataType.PpJson:
			// 	return $"{name} JSON NULL";

			case PpDataType.PpString:
				return $"{name} TEXT NULL";

			case PpDataType.PpStringArray:
				return $"{name} TEXT[] NULL";

			default:
				throw new InvalidOperationException($"Unsupported column '{column.PpDataType}'");
		}
	}

	public async Task<PpDbAppender> CreateAppenderAsync(PpTable table)
	{
		await using var cmd = await CreateCommandAsync();
		var appender = _conn.CreateAppender(table.TableName);

		return new PpDbAppender(appender, table);
	}

	// public async Task InsertDataAsync(PpTable table, IEnumerable<PpRecord> records)
	// {
	// 	var db = await CreateConnectionAsync();
	//
	// 	using var appender = db.CreateAppender(table.TableName);
	//
	// 	foreach (var rec in records)
	// 	{
	// 		var row = appender.CreateRow();
	// 		row.AppendValue(rec.AsJson());
	// 		row.EndRow();
	// 	}
	// }

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

	public async IAsyncEnumerable<PpRecord> RawQueryAsync(string query)
	{
		// _log.LogInformation("Executing query '{Query}' on table '{Table}'", query, table);

		// var db = await CreateConnectionAsync();
		// await using var cmd = db.CreateCommand();
		await using var cmd = await CreateCommandAsync();

		// cmd.CommandText = query.Replace("$table", $"\"{table.TableName}\"");
		cmd.CommandText = query;

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

	public async IAsyncEnumerable<PpRecord> QueryAsync(PpTable table, string query)
	{
		_log.LogInformation("Executing query '{Query}' on table '{Table}'", query, table);

		// var db = await CreateConnectionAsync();
		// await using var cmd = db.CreateCommand();
		await using var cmd = await CreateCommandAsync();

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

	public async IAsyncEnumerable<PpRecord> QueryAsync(IList<PpTable> tables, string query)
	{
		// _log.LogInformation("Executing query '{Query}' on table '{Table}'", query, tables);

		// var db = await CreateConnectionAsync();
		// await using var cmd = db.CreateCommand();
		await using var cmd = await CreateCommandAsync();

		cmd.CommandText = query;

		for (var i = 0; i < tables.Count; i++)
		{
			var table = tables[i];
			cmd.CommandText = cmd.CommandText.Replace($"$table{i}", $"\"{table.TableName}\"");
		}

		if (tables.Count > 0)
		{
			cmd.CommandText = cmd.CommandText.Replace("$table", $"\"{tables[0].TableName}\"");
		}

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
		// { typeof(object),			PpDataType.PpJson },
		{ typeof(string),			PpDataType.PpString },
		{ typeof(string[]),			PpDataType.PpStringArray },
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
		// { DuckDBType.Json, PpDataType.PpInt32 },
		{ DuckDBType.BigInt, PpDataType.PpInt64 },
		{ DuckDBType.Varchar, PpDataType.PpString },
		{ DuckDBType.Array, PpDataType.PpStringArray },
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

	// public async Task<ICollection<PpTable>> ListTablesAsync(CancellationToken ct = default)
	// {
	// 	await using var db = await CreateConnectionAsync();
	// 	await using var cmd = db.CreateCommand();
	//
	// 	var res = new List<PpTable>();
	//
	// 	cmd.CommandText = "show tables";
	//
	// 	var reader = await cmd.ExecuteReaderAsync(ct);
	// 	while (await reader.ReadAsync(ct))
	// 	{
	// 		var dict = new Dictionary<string, PpField>();
	//
	// 		for (var i = 0; i < reader.FieldCount; i++)
	// 		{
	// 			var name = reader.GetName(i);
	// 			var type = reader.GetFieldType(i);
	// 			// var val2 = reader.GetValue(i);
	// 			var val2 = reader.GetString(i);
	//
	// 			// dict[name] = new(ToPpDataType(type), val2);
	// 			var table = new PpTable(val2);
	// 			res.Add(table);
	//
	// 			await table.InitAsync();
	// 		}
	// 	}
	//
	// 	Tables = res;
	//
	// 	return res;
	// }
	public async Task ExecuteNonQueryAsync(string sql)
	{
		// var db = await CreateConnectionAsync();
		// await using var cmd = db.CreateCommand();
		await using var cmd = await CreateCommandAsync();

		cmd.CommandText = sql;

		await cmd.ExecuteNonQueryAsync();
	}

	public async Task InitTableAsync(PpTable table)
	{
		await OpenAsync();

		IEnumerable<DuckDbTableDescription> res = null!;

		try
		{
			res = await _conn.QueryAsync<DuckDbTableDescription>($"describe {table.TableName}");
		}
		catch (DuckDBException ex) when (ex.Message?.Contains("does not exist", StringComparison.OrdinalIgnoreCase) ?? false)
		{
			// return null;
		}

		// // var res = await db.GetSchemaAsync(name);
		// var cmd = db.CreateCommand();
		// cmd.CommandText = $"DESCRIBE {name}";
		// var reader = await cmd.ExecuteReaderAsync();

		// var table = new PpTable(name);

		foreach (var col in res)
		{
			table.Columns.Add(new(col.column_name, ToPpDataType(col.column_type)));
		}

		var dbg = 2;

		// return table;
	}

	public class DuckDbTableDescription
	{
		// [Column("column_name")]
		public string column_name { get; set; }

		public DuckDBType column_type { get; set; }
	}
}
