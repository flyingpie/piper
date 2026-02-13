namespace Piper.Core.Data;

/// <summary>
/// A "node input" is a port of a node, that accepts data from another node.
/// </summary>
public class PpNodeInput(PpNode node, string name) : PpNodePort(node, name)
{
	/// <summary>
	/// Convenience property to figure out whether an output is connected to this input.
	/// </summary>
	[MemberNotNullWhen(true, nameof(Output))]
	public bool IsConnected => Output != null;

	/// <summary>
	/// The output that this input is connected to (null if not connected).
	/// </summary>
	public PpNodeOutput? Output { get; set; }

	public PpTable? Table { get; set; }
}
