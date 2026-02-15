using Microsoft.Extensions.Logging;
using Piper.Core.Utils;

namespace Piper.Core.Data.Modifiers;

public class PpModifierStack
{
	private readonly ILogger _log = Log.For<PpModifierStack>();

	/// <summary>
	/// The table on which the stack of modifiers starts executing.
	/// </summary>
	public IPpTable BaseTable
	{
		get;
		set => field = value ?? PpNullTable.Instance;
	} = PpNullTable.Instance;

	public List<PpModifier> Modifiers { get; } = [];

	public IPpTable Table => Modifiers.LastOrDefault()?.Table ?? BaseTable;

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

		var table = BaseTable;

		foreach (var mod in Modifiers)
		{
			await mod.ExecuteAsync(table, ct);

			table = mod.Table;
		}
	}
}
