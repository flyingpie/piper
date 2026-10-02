using Piper.Core.Data;

namespace Piper.Core.Attributes;

/// <summary>
/// Used to decorate properties in nodes that represent incoming- or outgoing ports.<br/>
/// Although the type itself can already be used to infer this, there's additional metadata,
/// like the name of the port, as it should appear in the gui.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class PpPortAttribute(PpPortDirection direction, string name) : Attribute
{
	/// <summary>
	/// The name of the port, as it should appear in the GUI.
	/// </summary>
	public string Name { get; } = Guard.Against.NullOrWhiteSpace(name);

	public PpPortDirection Direction { get; } = direction;
}
