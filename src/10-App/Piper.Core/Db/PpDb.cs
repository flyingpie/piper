using Dapper;
using DuckDB.NET.Data;
using DuckDB.NET.Native;
using Piper.Core.Data;
using System.ComponentModel.DataAnnotations.Schema;

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

	public async Task V_CreateTableAsync(PpTable table)
	{
		await using var db = await CreateConnectionAsync();

		var sb1 = new StringBuilder();
		sb1.Append(
			$"""
			DROP TABLE IF EXISTS "{table.TableName}";
			CREATE TABLE "{table.TableName}"
			(
			""");

		foreach (var col in table.Columns)
		{
			sb1.Append(
				$"""
					{GetColThing(col)},

				""");
		}

		sb1.Append(
			$"""
			)
			""");

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

	public class DuckDbTableDescription
	{
		[Column("column_name")]
		public string column_name { get; set; }

		public DuckDBType column_type { get; set; }
	}

	public async Task V_InitTableAsync(PpTable table)
	{
		await using var db = await CreateConnectionAsync();

		// Dapper.SqlMapper.SetTypeMap

		IEnumerable<DuckDbTableDescription> res = null!;

		try
		{
			res = await db.QueryAsync<DuckDbTableDescription>($"describe {table.TableName}");
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

	public async Task<PpDbAppender> CreateAppenderAsync(PpTable table)
	{
		var db = await CreateConnectionAsync();
		var appender = db.CreateAppender(table.TableName);

		return new PpDbAppender(db, appender, table);
	}

	public async Task V_InsertDataAsync(PpTable table, IEnumerable<PpRecord> records)
	{
		await using var db = await CreateConnectionAsync();

		using var appender = db.CreateAppender(table.TableName);

		foreach (var rec in records)
		{
			var row = appender.CreateRow();

			foreach (var col in table.Columns)
			{
				if (!rec.Fields.TryGetValue(col.Name, out var val))
				{
					row.AppendNullValue();
					continue;
				}

				switch (val.Value)
				{
					case bool asBool:
						row.AppendValue(asBool);
						break;

					case DateTime asDt:
						row.AppendValue(asDt.ToUniversalTime());
						break;

					case float asFloat:
						row.AppendValue(asFloat);
						break;

					case Guid asGuid:
						row.AppendValue(asGuid);
						break;

					case int asInt:
						row.AppendValue(asInt);
						break;

					case long asLong:
						row.AppendValue(asLong);
						break;

					case string asString:
						row.AppendValue(asString);
						break;

					case null:
						row.AppendNullValue();
						break;

					default:
						throw new InvalidOperationException($"Unsupported data type '{val.Value.GetType().FullName}'.");
						break;
				}
			}

			row.EndRow();
		}
	}

	public async Task V_NonQueryRawAsync(string query)
	{
		await using var db = await CreateConnectionAsync();
		await using var cmd = db.CreateCommand();

		cmd.CommandText = query;

		await cmd.ExecuteNonQueryAsync();
	}

	public async IAsyncEnumerable<PpRecord> V_QueryRawAsync(string query)
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
				// var isNull = reader.IsDBNull(i);
				var val2 = reader.GetValue(i);
				// var val = isNull ? null : reader.GetString(i);

				if (val2 == DBNull.Value)
				{
					val2 = null;
				}

				dict[name] = new(ToPpDataType(type), val2);
			}

			yield return new PpRecord()
			{
				Fields = dict,
			};
		}
	}

	public async IAsyncEnumerable<PpRecord> V_QueryAsync(PpTable table, string query)
	{
		await using var db = await CreateConnectionAsync();
		await using var cmd = db.CreateCommand();

		cmd.CommandText = query
			.Replace("$table", $"\"{table.TableName}\"");

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

			yield return new PpRecord()
			{
				Fields = dict,
			};
		}
	}

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

	private static readonly Dictionary<DuckDBType, PpDataType> _duckTypeToPpType = new()
	{
		{ DuckDBType.Boolean,		PpDataType.PpBool },
		{ DuckDBType.Timestamp,		PpDataType.PpDateTime },
		{ DuckDBType.Double,		PpDataType.PpDouble },
		{ DuckDBType.Float,			PpDataType.PpFloat },
		{ DuckDBType.Uuid,			PpDataType.PpGuid },
		{ DuckDBType.Integer,		PpDataType.PpInt32 },
		{ DuckDBType.BigInt,		PpDataType.PpInt64 },
		{ DuckDBType.Varchar,		PpDataType.PpString },
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
			DROP TABLE IF EXISTS "{table.TableName}";
			CREATE TABLE "{table.TableName}"
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
		var db = new DuckDBConnection("data source=/home/marco/Downloads/piper.db");
		await db.OpenAsync();

		return db;
	}
}