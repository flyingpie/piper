using Piper.Core.Db;

namespace Piper.Core;

public class PpTable
{
	private Action<PpTable>? _onChange;

	public string TableName { get; set; }

	public List<PpColumn> Columns { get; set; } = [];

	// public IEnumerable<PpRow> Rows { get; }

	public void OnChange(Action<PpTable> onChange)
	{
		_onChange = onChange;
	}

	public async Task ClearAsync()
	{
		await DuckDbPpDb.Instance.CreateTableAsync(TableName, Columns);

		_onChange?.Invoke(this);
	}

	public async Task AddAsync(PpRecord record)
	{
		await AddAsync([record]);
	}

	public async Task AddAsync(IEnumerable<PpRecord> records)
	{
		ArgumentNullException.ThrowIfNull(records);

		var db = DuckDbPpDb.Instance;

		await db.InsertDataAsync(TableName, records);

		_onChange?.Invoke(this);
	}

	public async Task<ICollection<PpRecord>> QueryAsync(string sql)
	{
		return null;
	}
}

public class PpColumn
{
	public string Name { get; set; }

	public int Type { get; set; }
}