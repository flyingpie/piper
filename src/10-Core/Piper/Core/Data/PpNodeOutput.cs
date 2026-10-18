using Piper.Core.Modifiers;
using Piper.Core.Nodes;

namespace Piper.Core.Data;

/// <summary>
/// A table coming out of a node, owned by the node itself.<br/>
/// A "node output" is a port of a node, that accepts data from another node.
/// </summary>
public class PpNodeOutput : PpNodePort
{
	public PpNodeOutput(PpNode node, string name)
		: base(node, name)
	{
		BaseTable = new PpTable();
		Modifiers = new(() => BaseTable);
	}

	/// <summary>
	/// The table that the owning node dumps data in.
	/// </summary>
	public IPpTable BaseTable { get; }

	/// <summary>
	/// Modifier stack on top of the above base table.
	/// </summary>
	public override PpModifierStack Modifiers { get; }
}
