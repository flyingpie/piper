using Piper.Core.Nodes;

namespace Piper.Core.Data;

/// <summary>
/// A table coming out of a node, owned by the node itself.
/// </summary>
public class PpNodeOutput(PpNodeBase node, string name)
{
	public PpNodeBase Node { get; } = Guard.Against.Null(node);

	public string Name { get; } = Guard.Against.NullOrWhiteSpace(name);

	public Func<PpTable> Table { get; set; }
}