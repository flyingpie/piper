namespace Piper.Core.Data;

/// <summary>
/// Base class for <see cref="PpNodeInput"/> and <see cref="PpNodeOutput"/>.
/// </summary>
public abstract class PpNodePort(PpNode node, string name)
{
	/// <summary>
	/// The name of the port.
	/// </summary>
	public string Name { get; } = Guard.Against.NullOrWhiteSpace(name);

	/// <summary>
	/// The node owning this port.
	/// </summary>
	public PpNode Node { get; } = Guard.Against.Null(node);
}
