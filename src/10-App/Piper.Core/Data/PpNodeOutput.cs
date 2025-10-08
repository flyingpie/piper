namespace Piper.Core.Data;

/// <summary>
/// A table coming out of a node, owned by the node itself.
/// </summary>
public class PpNodeOutput
{
	public Func<PpTable> Table { get; set; }
}