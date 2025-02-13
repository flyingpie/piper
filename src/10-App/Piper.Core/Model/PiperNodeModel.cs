using System.Drawing;
using System.Numerics;
using Tomlet.Attributes;

namespace Piper.Core.Model;

[Tomlet.Attributes.TomlDoNotInlineObject]
public class PiperNodeModel
{
	// public Guid Id { get; set; } = Guid.NewGuid();

	[TomlNonSerialized]
	public string? Name { get; set; } = "Node name 1";

	//public Point Position { get; set; }

	public List<PiperPortModel> InPorts { get; set; } = [];

	public List<PiperPortModel> OutPorts { get; set; } = [];
}
