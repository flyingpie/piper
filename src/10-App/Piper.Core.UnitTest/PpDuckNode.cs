using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DuckDB.NET.Data;

namespace Piper.Core.UnitTest;

public class PpDuckNode : IPpNode
{
	private PpDataFrame _outLines = new();

	public string? Name { get; set; }

	public PpNodeInput In { get; set; } = new();

	public PpNodeOutput OutIncl { get; } = new();

	public string Query { get; set; }

	public async Task ExecuteAsync()
	{
		// Prep frame
		_outLines.Records.Clear();

		var frame = In.DataFrame();
		var cols = frame.FieldNames.ToList();

		// TODO: Execute DuckDB SQL
		await using var db = new DuckDBConnection("data source=piper.db");
		await db.OpenAsync();

		await CreateTableAsync(db, cols);

		// Insert data
		InsertDataAsync(db, cols, frame);

		// Read data
		_outLines = await ReadDataAsync(db, Query);

		OutIncl.DataFrame = () => _outLines;
	}

	private static void InsertDataAsync(DuckDBConnection db, List<string> cols, PpDataFrame frame)
	{
		using var appender = db.CreateAppender("t1");

		foreach (var rec in frame.Records)
		{
			var row = appender.CreateRow();
			foreach (var col in cols)
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
}