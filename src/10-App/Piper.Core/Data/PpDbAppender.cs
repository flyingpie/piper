using DuckDB.NET.Data;

namespace Piper.Core.Data;

public sealed class PpDbAppender(DuckDBConnection conn, DuckDBAppender appender, PpTable table) : IAsyncDisposable
{
	private readonly DuckDBConnection _conn = Guard.Against.Null(conn);
	private readonly DuckDBAppender _appender = Guard.Against.Null(appender);
	private readonly PpTable _table = Guard.Against.Null(table);

	private int _i;

	public void AddRange(IEnumerable<PpRecord> records)
	{
		foreach (var record in records)
		{
			Add(record);
		}
	}

	public void Add(string json)
	{
		var row = appender.CreateRow();
		row.AppendValue(json);
		row.EndRow();
	}

	public void Add(PpRecord record)
	{
		Guard.Against.Null(record);

		if (++_i >= 1000)
		{
			// _table.Changed();
			_i = 0;
		}

		var row = appender.CreateRow();

		foreach (var col in _table.Columns)
		{
			if (!record.Fields.TryGetValue(col.Name, out var val))
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

				case double asDouble:
					row.AppendValue(asDouble);
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
			}
		}

		row.EndRow();
	}

	public async ValueTask DisposeAsync()
	{
		_appender.Dispose();
		await _conn.DisposeAsync();
	}
}
