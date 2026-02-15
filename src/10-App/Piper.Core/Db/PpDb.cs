using Dapper;
using DuckDB.NET.Data;
using DuckDB.NET.Native;
using Microsoft.Extensions.Logging;
using Piper.Core.Data;
using Piper.Core.Utils;

namespace Piper.Core.Db;

public interface IPpDb
{
	/// <summary>
	/// Provides direct access to the underlying DuckDB instance.
	/// </summary>
	IPpDbLowLevel LowLevel { get; }

	/// <summary>
	/// Creates a table in the database, based on the structure as describe by the specified <paramref name="table"/>.<br/>
	/// So PpTable -> DuckDB.
	/// </summary>
	Task CreateTableAsync(IPpTable table, CancellationToken ct = default);

	/// <summary>
	/// Loads a structure from the database, and stores it in the specified <paramref name="table"/>.<br/>
	/// So DuckDB -> PpTable.
	/// </summary>
	Task FetchTableAsync(IPpTable table, CancellationToken ct = default);

	/// <summary>
	/// TODO
	/// </summary>
	Task<long> CountAsync(IPpTable table, CancellationToken ct = default);

	/// <summary>
	/// Executes a query on the specified <paramref name="table"/>.
	/// The table will be available as a query source, using the variable '$table'.
	/// </summary>
	IAsyncEnumerable<PpRecord> QueryAsync(IPpTable table, string query, CancellationToken ct = default);

	/// <summary>
	/// Executes a query on the specified <paramref name="tables"/>.<br/>
	/// Each table will be available as a query source, using the variable '$table0', '$table1', etc.
	/// </summary>
	IAsyncEnumerable<PpRecord> QueryAsync(IList<IPpTable> tables, string query, CancellationToken ct = default);

	/// <summary>
	/// Creates an appender, that can be used to efficiently bulk insert into the specified <paramref name="table"/>.
	/// </summary>
	Task<PpDbAppender> CreateAppenderAsync(IPpTable table, CancellationToken ct = default);
}

public class PpDb : IPpDb
{
	private readonly ILogger _log = Log.For<PpDb>();
	private readonly DuckDBConnection _conn = new("DataSource=:memory:?cache=shared");
	private readonly SemaphoreSlim _lock = new(1);

	private bool _isOpen;

	private PpDb()
	{
		LowLevel = new PpDbLowLevel(CreateCommandAsync);
	}

	public static IPpDb Instance { get; } = new PpDb();

	/// <inheritdoc/>
	public IPpDbLowLevel LowLevel { get; }

	/// <inheritdoc/>
	public async Task<long> CountAsync(IPpTable table, CancellationToken ct = default)
	{
		Guard.Against.Null(table);

		return await LowLevel.ExecuteScalarAsync($"select count(1) from {table.Name}", ct);
	}

	/// <inheritdoc/>
	public async Task<PpDbAppender> CreateAppenderAsync(IPpTable table, CancellationToken ct = default)
	{
		await using var cmd = await CreateCommandAsync();
		var appender = _conn.CreateAppender(table.Name);

		return new PpDbAppender(appender, table);
	}

	/// <inheritdoc/>
	public async Task CreateTableAsync(IPpTable table, CancellationToken ct = default)
	{
		var sb1 = new StringBuilder();
		sb1.Append(
			$"""
			DROP TABLE IF EXISTS "{table.Name}";
			CREATE TABLE "{table.Name}"
			(
			"""
		);

		foreach (var col in table.Columns)
		{
			sb1.Append(
				$"""
					{col.ToDuckDbColumnSql()},

				"""
			);
		}

		sb1.Append(
			"""
			)
			"""
		);

		await LowLevel.ExecuteNonQueryAsync(sb1.ToString());
	}

	/// <inheritdoc/>
	public async Task FetchTableAsync(IPpTable table, CancellationToken ct = default)
	{
		await OpenAsync();

		IEnumerable<DuckDbTableDescription> res = null!;

		try
		{
			res = await _conn.QueryAsync<DuckDbTableDescription>($"describe {table.Name}");
		}
		catch (DuckDBException ex) when (ex.Message?.Contains("does not exist", StringComparison.OrdinalIgnoreCase) ?? false)
		{
			// TODO
		}

		foreach (var col in res ?? [])
		{
			table.Columns.Add(new(col.column_name, col.column_type.ToPpDataType()));
		}
	}

	/// <inheritdoc/>
	public IAsyncEnumerable<PpRecord> QueryAsync(IPpTable table, string query, CancellationToken ct = default)
	{
		_log.LogInformation("Executing query '{Query}' on table '{Table}'", query, table);

		return QueryAsync([table], query);

		// await using var cmd = await CreateCommandAsync();
		//
		// cmd.CommandText = query.Replace("$table", $"\"{table.TableName}\"");
		//
		// var reader = await cmd.ExecuteReaderAsync();
		// while (await reader.ReadAsync())
		// {
		// 	var dict = new Dictionary<string, PpField>();
		//
		// 	for (var i = 0; i < reader.FieldCount; i++)
		// 	{
		// 		var name = reader.GetName(i);
		// 		var type = reader.GetFieldType(i);
		// 		var val2 = reader.GetValue(i);
		//
		// 		dict[name] = new(type.ToPpDataType(), val2);
		// 	}
		//
		// 	yield return new PpRecord() { Fields = dict };
		// }
	}

	/// <inheritdoc/>
	public async IAsyncEnumerable<PpRecord> QueryAsync(IList<IPpTable> tables, string query, CancellationToken ct = default)
	{
		await using var cmd = await CreateCommandAsync();

		cmd.CommandText = query;

		for (var i = 0; i < tables.Count; i++)
		{
			var table = tables[i];
			cmd.CommandText = cmd.CommandText.Replace($"$table{i}", $"\"{table.Name}\"");
		}

		if (tables.Count > 0)
		{
			cmd.CommandText = cmd.CommandText.Replace("$table", $"\"{tables[0].Name}\"");
		}

		var reader = await cmd.ExecuteReaderAsync();
		while (await reader.ReadAsync())
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

	#region Connection

	private async Task OpenAsync()
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

	private async Task<DuckDBCommand> CreateCommandAsync()
	{
		await OpenAsync();

		return _conn.CreateCommand();
	}

	#endregion


	private sealed class DuckDbTableDescription
	{
		public string column_name { get; set; }

		public DuckDBType column_type { get; set; }
	}
}
