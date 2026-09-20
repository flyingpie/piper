namespace Piper.Core.Data;

/// <summary>
/// A table coming out of a node, owned by the node itself.
/// A "node output" is a port of a node, that accepts data from another node.
/// </summary>
public class PpNodeOutput : PpNodePort
{
	public PpNodeOutput(PpNode node, string name)
		: base(node, name)
	{
		BaseTable = new PpTable();
		Modifiers.BaseTable = () => BaseTable;
	}

	public IPpTable BaseTable { get; }
}
