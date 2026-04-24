using DuckDB.NET.Data;

namespace Piper.Core.Data;

public sealed class PpDbAppender(
	// DuckDBAppender appender,
	Func<Task<DuckDBAppender>> appenderFactory,
	IPpTable table
) : IAsyncDisposable
{
	// private readonly DuckDBAppender _appender = Guard.Against.Null(appender);
	private readonly Func<Task<DuckDBAppender>> _appenderFactory = Guard.Against.Null(appenderFactory);
	private readonly PpTable _table = (PpTable)Guard.Against.Null(table);

	private DuckDBAppender? _appender;

	private int _i;

	public void AddRange(IEnumerable<PpRecord> records)
	{
		foreach (var record in records)
		{
			Add(record);
		}
	}

	// public void Add(string json)
	// {
	// 	var row = appender.CreateRow();
	// 	row.AppendValue(json);
	// 	row.EndRow();
	// }

	public void Add(PpRecord record)
	{
		Guard.Against.Null(record);

		if (++_i >= 1000)
		{
			// _table.Changed();
			_i = 0;
		}

		if (_appender == null)
		{
			_table.Init(record);
			_appender = appenderFactory().GetAwaiter().GetResult(); // TODO: Fix
		}

		var row = _appender.CreateRow();

		foreach (var col in _table.Columns)
		{
			if (!record.TryGetField(col.Name, out var field))
			{
				row.AppendNullValue();
				continue;
			}

			switch (field.Value)
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

				case List<string> asStringArray:
					row.AppendValue(asStringArray);
					break;

				case DBNull:
				case null:
					row.AppendNullValue();
					break;

				default:
					throw new InvalidOperationException($"Unsupported data type '{field.Value.GetType().FullName}'.");
			}
		}

		row.EndRow();
	}

	public async ValueTask DisposeAsync()
	{
		_appender?.Dispose();
		// await _conn.DisposeAsync();
	}
}
