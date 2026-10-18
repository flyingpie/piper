using Microsoft.Extensions.Logging;
using Piper.Utils;

namespace Piper.Core.Modifiers;

/// <summary>
/// A "modifier stack" starts with a "base table"; a table that forms the source of records.<br/>
/// <br/>
/// Then multiple "modifiers" can be added, each taking in the previous table (starting with the base table),
/// doing something with it, and outputting its own table as a result.<br/>
/// <br/>
/// Finally, the resulting table can be queried, which will show the results of the entire stack.
/// </summary>
public sealed class PpModifierStack(Func<IPpTable> baseTableFunc)
{
	private readonly ILogger _log = Log.For<PpModifierStack>();

	/// <summary>
	/// The table on which the stack of modifiers starts executing.
	/// </summary>
	public Func<IPpTable> BaseTable { get; } = Guard.Against.Null(baseTableFunc);

	public List<PpModifier> Modifiers { get; } = [];

	public IPpTable Table => Modifiers.LastOrDefault()?.Table ?? BaseTable();

	public void Add(PpModifier mod)
	{
		Modifiers.Add(mod);
	}

	public void Remove(PpModifier mod)
	{
		Modifiers.Remove(mod);
	}

	public async Task ExecuteAsync(CancellationToken ct = default)
	{
		_log.LogInformation("Executing modifier stack");

		var table = BaseTable();

		foreach (var mod in Modifiers)
		{
			await mod.ExecuteAsync(table, ct);

			table = mod.Table;
			await table.DoneAsync(ct);
		}
	}
}
