using Piper.Core.Db;

namespace Piper.Core.Data;

/// <inheritdoc cref="IPpTable"/>
public class PpTable(string? name = null, ICollection<PpColumn>? columns = null) : IPpTable
{
	/// <inheritdoc/>
	public long Count { get; set; }

	public bool IsInitialized { get; private set; }

	/// <inheritdoc/>
	public string Name { get; } = Guard.Against.NullOrWhiteSpace(name ?? PpId.Instance.NextTable());

	/// <inheritdoc/>
	public List<PpColumn> Columns { get; set; } = (columns ?? []).ToList();

	/// <inheritdoc/>
	public async Task<IPpTable> AddAsync(params IEnumerable<PpRecord> records)
	{
		await using var appender = await CreateAppenderAsync();
		appender.AddRange(records);

		return this;
	}

	/// <inheritdoc/>
	public IPpTable Clear()
	{
		IsInitialized = false;

		// await PpDb.Instance.CreateTableAsync(this, ct);

		// Count = await CountAsync(ct);
		// Count = await PpDb.Instance.CountAsync(this, ct);

		return this;
	}

	/// <inheritdoc/>
	public async Task<IPpTable> ClearAsync(CancellationToken ct = default)
	{
		IsInitialized = false;

		// await PpDb.Instance.CreateTableAsync(this, ct);

		// Count = await CountAsync(ct);
		// Count = await PpDb.Instance.CountAsync(this, ct);

		return this;
	}

	/// <inheritdoc/>
	public async Task FetchAsync(CancellationToken ct = default)
	{
		await PpDb.Instance.FetchTableAsync(this, ct);

		await DoneAsync(ct);
	}

	/// <inheritdoc/>
	public async Task<IPpTable> DoneAsync(CancellationToken ct = default)
	{
		if (IsInitialized)
		{
			// Count = await CountAsync(ct);
			Count = await PpDb.Instance.CountAsync(this, ct);
		}

		return this;
	}

	public void Init(IEnumerable<PpColumn> columns, bool createTable = true)
	{
		// Guard.Against.Null(record);

		if (IsInitialized)
		{
			return;
		}

		Columns.Clear();
		Columns.AddRange(columns);

		if (createTable)
		{
			PpDb.Instance.CreateTableAsync(this).GetAwaiter().GetResult();
		}

		IsInitialized = true;
	}

	public void Init(PpRecord record, bool createTable = true)
	{
		Guard.Against.Null(record);

		if (IsInitialized)
		{
			return;
		}

		Columns.Clear();
		Columns.AddRange(record.Select(rec => rec.AsColumn()));

		if (createTable)
		{
			PpDb.Instance.CreateTableAsync(this).GetAwaiter().GetResult();
		}

		IsInitialized = true;
	}

	// /// <inheritdoc/>
	// public static async Task<PpTable> CreateAsync(params PpRecord[] records)
	// {
	// 	var cols = records[0](r => r.Fields2.Select(f => f.AsColumn()));
	// 	var table = new PpTable(null, cols);
	//
	// 	await PpDb.Instance.CreateTableAsync(table);
	//
	// 	// Count = await CountAsync();
	//
	// 	return table;
	// }

	// /// <inheritdoc/>
	// public static async Task<IPpTable> CreateAsync(PpTable from, params PpRecord[] records)
	// {
	// 	var table = new PpTable();
	//
	// 	await PpDb.Instance.CreateTableAsync(table);
	//
	// 	// Count = await CountAsync();
	//
	// 	return table;
	// }

	// /// <inheritdoc/>
	// public async Task<IPpTable> RecreateAsync(CancellationToken ct = default)
	// {
	// 	await PpDb.Instance.CreateTableAsync(this, ct);
	//
	// 	Count = await CountAsync(ct);
	//
	// 	return this;
	// }

	/// <inheritdoc/>
	// public Task<long> CountAsync(CancellationToken ct = default) => PpDb.Instance.CountAsync(this, ct);

	/// <inheritdoc/>
	public IAsyncEnumerable<PpRecord> QueryAllAsync(CancellationToken ct = default)
	{
		if (!IsInitialized)
		{
			return AsyncEnumerable.Empty<PpRecord>();
		}

		return QueryAsync($"select * from {Name}", ct);
	}

	/// <inheritdoc/>
	public IAsyncEnumerable<PpRecord> QueryAsync(string sql, CancellationToken ct = default)
	{
		if (!IsInitialized)
		{
			return AsyncEnumerable.Empty<PpRecord>();
		}

		return PpDb.Instance.QueryAsync(this, sql, ct);
	}

	/// <inheritdoc/>
	public Task<PpDbAppender> CreateAppenderAsync(CancellationToken ct = default)
	{
		return PpDb.Instance.CreateAppenderAsync(this, ct);
	}

	// public PpTable WithRecords(params PpRecord[] record)
	// {
	// 	throw new NotImplementedException();
	// }

	// public IPpTable WithRecords()
	// {
	// 	throw new NotImplementedException();
	// }

	// public PpTable Clear()
	// {
	// 	ClearColumns();
	//
	// 	return this;
	// }

	// public PpTable ClearColumns()
	// {
	// 	Columns.Clear();
	//
	// 	return this;
	// }

	// public PpTable WithColumns(params IEnumerable<PpColumn> columns)
	// {
	// 	Columns.AddRange(columns);
	//
	// 	return this;
	// }

	//
	// public PpTable WithColumns() { }
	//
	// public PpTable WithRecord(params PpRecord[] record);
	//
	// public PpTable WithRecord(IDictionary<string, PpField> record);
	//
	// public PpTable WithRecords(params IEnumerable<PpRecord> records);
}

// public class PpTableBuilder
// {
// 	// private PpTableBuilder() { }
//
// 	// public PpTableBuilder New() => new();
//
// 	public PpTableBuilder WithColumns(params IEnumerable<PpColumn> columns) { }
//
// 	public IPpTable WithColumns() { }
//
// 	public IPpTable WithRecord(params PpRecord[] record);
//
// 	public IPpTable WithRecord(IDictionary<string, PpField> record);
//
// 	public IPpTable WithRecords(params IEnumerable<PpRecord> records);
// }
