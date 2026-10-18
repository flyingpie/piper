namespace Piper.Core;

/// <summary>
/// Generates ids to use with elements like nodes and tables, assigning a unique,
/// incremental id to everyone.<br/>
/// <br/>
/// We're doing incremental integers as opposed to e.g. GUIDs, to keep the ids shorter.
/// </summary>
public class PpId
{
	/// <summary>
	/// Counter, we're sharing a single one for all id types just to keep things simple.
	/// </summary>
	private int _i;

	public static PpId Instance { get; } = new();

	public string NextMod() => $"mod{Interlocked.Increment(ref _i):0000}";

	public string NextNode() => $"node{Interlocked.Increment(ref _i):0000}";

	public string NextTable() => $"tbl{Interlocked.Increment(ref _i):0000}";

	public void Reset() => _i = 0;
}
