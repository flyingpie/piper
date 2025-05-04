using Tomlet.Attributes;

namespace Piper.Core.Model;

public enum PiperPortDirection
{
	None = 0,
	In,
	Out,
}

//[TomlDoNotInlineObject]
public class PiperPortModel
{
	[TomlNonSerialized]
	public PiperPortDirection Direction { get; set; }

	// [TomlProperty("name")]
	[TomlNonSerialized]
	public string? Name { get; set; }

	// TODO: Can we keep the pointers out of the model, and just fetch them when necessary?
	// TODO: Would make (de)serialization a lot easier, and keep the state simpler.
	// public List<PiperLinkModel> Links { get; set; } = [];
}

public class PiperIntPortModel : PiperPortModel
{
	[TomlProperty("max")]
	public long? Max { get; set; }

	[TomlProperty("max_time")]
	public TimeSpan MaxTime { get; set; }
}

public class PiperStringPortModel : PiperPortModel
{
	[TomlProperty("delimiter")]
	public string? Delimiter { get; set; }
}

public class PiperPortModelRef { }
