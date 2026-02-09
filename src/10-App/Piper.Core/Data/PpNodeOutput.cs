namespace Piper.Core.Data;

/// <summary>
/// A table coming out of a node, owned by the node itself.
/// A "node output" is a port of a node, that accepts data from another node.
/// </summary>
public class PpNodeOutput(PpNode node, string name, PpTable table) : PpNodePort(node, name)
{
	public PpTable Table { get; } = Guard.Against.Null(table);
}
