using Piper.Core.Db;
using System.Threading;

namespace Piper.Core;

public class PpTable
{
	private Action<PpTable>? _onChange;

	public string TableName { get; set; }

	public List<PpColumn> Columns { get; set; } = [];

	public void OnChange(Action<PpTable> onChange)
	{
		_onChange = onChange;
	}

	public async Task ClearAsync(CancellationToken ct = default)
	{
		await DuckDbPpDb.Instance.CreateTableAsync(this);

		_onChange?.Invoke(this);
	}

	public async Task AddAsync(PpRecord record)
	{
		Guard.Against.Null(record);

		await AddAsync([record]);
	}

	public async Task AddAsync(IEnumerable<PpRecord> records)
	{
		ArgumentNullException.ThrowIfNull(records);

		var db = DuckDbPpDb.Instance;

		await db.InsertDataAsync(TableName, records);

		_onChange?.Invoke(this);
	}

	public IAsyncEnumerable<PpRecord> QueryAllAsync()
	{
		return DuckDbPpDb.Instance.QueryAsync($"select * from {TableName}");
	}

	public IAsyncEnumerable<PpRecord> QueryAsync(string sql)
	{
		return DuckDbPpDb.Instance.QueryAsync(sql);
	}
}