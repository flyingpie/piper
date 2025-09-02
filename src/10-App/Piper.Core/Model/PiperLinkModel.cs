using shortid;
using Tomlet.Attributes;

namespace Piper.Core.Model;

[TomlDoNotInlineObject]
public class PiperLinkModel
{
	[TomlProperty("id")]
	public string Id { get; set; } = ShortId.Generate();

	[TomlProperty("src")]
	public PiperLinkModelRef From { get; set; } = new();

	[TomlProperty("dst")]
	public PiperLinkModelRef To { get; set; } = new();

	[TomlNonSerialized]
	public List<PiperAnchor> Anchors { get; set; } = [new(), new()];

	[TomlProperty("anchors")]
	public Dictionary<int, PiperAnchor> Anchors1 =>
		Anchors.Select((a, i) => (a, i)).ToDictionary(x => x.i, x => x.a);
}

public class PiperLinkModelRef
{
	[TomlProperty("id")]
	public string Id { get; set; } = ShortId.Generate();

	[TomlProperty("is_thingy")]
	public bool IsThingy { get; set; } = true;
}

public struct PiperAnchor
{
	[TomlProperty("pos")]
	public System.Drawing.Point Position { get; set; }

	[TomlProperty("cp1")]
	public System.Drawing.Point ControlPoint1 { get; set; }

	[TomlProperty("cp2")]
	public System.Drawing.Point ControlPoint2 { get; set; }
}
