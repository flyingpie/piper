using Piper.Core.Db;
using System.Threading;

namespace Piper.Core.Data;

public class PpTable(string tableName, ICollection<PpColumn> columns)
{
	private Action<PpTable>? _onChange;

	public PpTable(string tableName)
		: this(tableName, [])
	{
	}

	public long Count { get; set; }

	public string TableName { get; } = Guard.Against.NullOrWhiteSpace(tableName);

	public List<PpColumn> Columns { get; set; } = Guard.Against.Null(columns).ToList();

	public void OnChange(Action<PpTable> onChange)
	{
		_onChange = onChange;
	}

	public async Task ClearAsync(CancellationToken ct = default)
	{
		// await PpDb.Instance.CreateTableAsync(this);
		await PpDb.Instance.V_CreateTableAsync(this);

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

		var db = PpDb.Instance;

		// await db.InsertDataAsync(TableName, records);
		await db.V_InsertDataAsync(this, records);
	}

	public async Task DoneAsync()
	{
		Count = await CountAsync();

		_onChange?.Invoke(this);
	}

	public async Task<long> CountAsync()
	{
		return await PpDb.Instance.CountAsync($"select count(1) from {TableName}");
	}

	public IAsyncEnumerable<PpRecord> QueryAllAsync()
	{
		// return PpDb.Instance.QueryAsync($"select * from {TableName}");
		return PpDb.Instance.V_QueryAsync(this, "select * from $table");
	}

	public IAsyncEnumerable<PpRecord> QueryAsync(string sql)
	{
		// return PpDb.Instance.QueryAsync(sql);
		return PpDb.Instance.V_QueryAsync(this, sql);
	}
}