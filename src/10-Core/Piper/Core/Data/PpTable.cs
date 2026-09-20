using Microsoft.Extensions.Logging;

namespace Piper.Core.Data;

/// <inheritdoc cref="IPpTable"/>
public sealed class PpTable : IPpTable
{
	private readonly ILogger _log = Log.For<PpTable>();

	private readonly List<PpColumn> _columns = [];

	/// <summary>
	/// Determines whether we have a link to the table (or view) in the database yet.<br/>
	/// If this is false, the table needs to be created first.
	/// </summary>
	private bool _isInitialized;

	/// <inheritdoc/>
	public IEnumerable<PpColumn> Columns => _columns;

	/// <inheritdoc/>
	public long Count { get; private set; }

	/// <inheritdoc/>
	public string Name { get; } = PpId.Instance.NextTable();

	/// <inheritdoc/>
	public async Task<IPpTable> AddAsync(params ICollection<PpRecord> records)
	{
		_log.LogDebug("[PpTable:{Table}] Adding {Count} rows", Name, records);

		var sw = Stopwatch.StartNew();

		try
		{
			await using var appender = await CreateAppenderAsync();
			await appender.AddRangeAsync(records);

			_log.LogDebug("[PpTable:{Table}] Added {Count} rows, took {Elapsed}", Name, records.Count, sw.Elapsed);
		}
		catch (Exception ex)
		{
			_log.LogError(ex, "[PpTable:{Table}] Error adding rows: {Message}, took {Elapsed}", Name, ex.Message, sw.Elapsed);
			throw;
		}

		return this;
	}

	/// <inheritdoc/>
	public IPpTable Clear()
	{
		_log.LogDebug("[PpTable:{Table}] Clearing", Name);

		// Mark the table to be recreated on the next operation.
		_isInitialized = false;

		return this;
	}

	/// <inheritdoc/>
	public Task<PpDbAppender> CreateAppenderAsync(CancellationToken ct = default)
	{
		return PpDb.Instance.CreateAppenderAsync(this, ct);
	}

	/// <inheritdoc/>
	public async Task<IPpTable> DoneAsync(CancellationToken ct = default)
	{
		if (!_isInitialized)
		{
			throw new InvalidOperationException($"Attempting to call {nameof(DoneAsync)} on uninitialized table '{this}'.");
		}

		// Update count.
		Count = await PpDb.Instance.CountAsync(this, ct);

		return this;
	}

	/// <inheritdoc/>
	public async Task InitAsync(IEnumerable<PpColumn> columns, bool createTable = true, CancellationToken ct = default)
	{
		// Don't initialize if we're already initialized.
		if (_isInitialized)
		{
			return;
		}

		_columns.Clear();
		_columns.AddRange(columns);

		if (createTable)
		{
			await PpDb.Instance.CreateTableAsync(this);
		}

		_isInitialized = true;
	}

	/// <inheritdoc/>
	public async Task InitFromDbAsync(CancellationToken ct = default)
	{
		var columns = await PpDb.Instance.GetColumnsAsync(Name, ct);

		_columns.Clear();
		_columns.AddRange(columns);

		await DoneAsync(ct);
	}

	/// <inheritdoc/>
	public IAsyncEnumerable<PpRecord> QueryAllAsync(CancellationToken ct = default)
	{
		if (!_isInitialized)
		{
			return AsyncEnumerable.Empty<PpRecord>();
		}

		return QueryAsync($"select * from {Name}", ct);
	}

	/// <inheritdoc/>
	public IAsyncEnumerable<PpRecord> QueryAsync(string sql, CancellationToken ct = default)
	{
		if (!_isInitialized)
		{
			return AsyncEnumerable.Empty<PpRecord>();
		}

		return PpDb.Instance.QueryAsync(this, sql, ct);
	}

	// [LoggerMessage(EventId = 0, Level = LogLevel.Critical, Message = "Could not open socket to `{HostName}`")]
	// public partial void CouldNotOpenSocket(ILogger logger, string hostName, int a);
}
