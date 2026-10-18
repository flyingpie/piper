using Piper.Core.Nodes;

namespace Piper.Core.Attributes;

/// <summary>
/// Used to decorate properties that represent node parameters that can be changed by the user.<br/>
/// Although the type itself can already be used to infer this, there's additional metadata,
/// like the name of the port, as it should appear in the gui.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class PpParamAttribute(string name) : Attribute
{
	/// <summary>
	/// The name of the parameter, as it should appear in the GUI.
	/// </summary>
	public string Name { get; } = Guard.Against.NullOrWhiteSpace(name);

	/// <summary>
	/// A flag used to add semantic data to a parameter, like classify a string as a hex color,
	/// or suggesting single vs multi-line.
	/// </summary>
	public PpParamHint Hint { get; set; } = PpParamHint.None;
}
