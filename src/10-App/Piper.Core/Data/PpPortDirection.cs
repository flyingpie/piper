namespace Piper.Core.Data;

/// <summary>
/// Annotates ports with a data flow direction, for example to know on which side of a node to display the port.
/// </summary>
public enum PpPortDirection
{
	/// <summary>
	/// For detecting serialization issues.
	/// </summary>
	None = 0,

	/// <summary>
	/// Data goes _into_ this port, from another node.
	/// </summary>
	In,

	/// <summary>
	/// Data goes _out_ of this port, to another node.
	/// </summary>
	Out,
}
