using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Tomlet.Attributes;

namespace Piper.Core.Model;

public enum PiperPortDirection
{
	None = 0,
	In,
	Out,
}

//[TomlDoNotInlineObject]
public class PiperPortModel : PortModel
{
	public PiperPortModel(string? name, NodeModel parent, PortAlignment alignment = PortAlignment.Bottom, Point? position = null, Size? size = null) : base(parent, alignment, position, size)
	{
		// Direction = direction;
		Name = name;
	}

	public PiperPortModel(string? name, string id, NodeModel parent, PortAlignment alignment = PortAlignment.Bottom, Point? position = null, Size? size = null) : base(id, parent, alignment, position, size)
	{
		// Direction = direction;
		Name = name;
	}

	// [TomlNonSerialized]
	// public PiperPortDirection Direction { get; set; }

	// [TomlProperty("name")]
	[TomlNonSerialized]
	public string? Name { get; set; }

	// TODO: Can we keep the pointers out of the model, and just fetch them when necessary?
	// TODO: Would make (de)serialization a lot easier, and keep the state simpler.
	// public List<PiperLinkModel> Links { get; set; } = [];
}

// public class PiperIntPortModel : PiperPortModel
// {
// 	[TomlProperty("max")]
// 	public long? Max { get; set; }
//
// 	[TomlProperty("max_time")]
// 	public TimeSpan MaxTime { get; set; }
// }
//
// public class PiperStringPortModel : PiperPortModel
// {
// 	[TomlProperty("delimiter")]
// 	public string? Delimiter { get; set; }
// }
//
// public class PiperPortModelRef
// {
// }