using System.Text.Json.Serialization;
using Blazor.Diagrams;
using Piper.Core.Model;
using shortid;
using Tomlet.Attributes;

namespace Piper.Core;

public class PiperGraph
{
	[TomlPrecedingComment("\nNodes\n")]
	[TomlProperty("node")]
	public List<PiperNodeModel> Nodes { get; set; } =
		[
			new RunProcessPiperNodeModel() { Name = "Node 1" },
			new() { Name = "Node 2" },
			new() { Name = "Node 3" },
		];

	// public HashSet<PiperNodeModel> Nodes { get; set; } = new HashSet<PiperNodeModel>();

	// public Dictionary<string, PiperNodeModel> Nodes2 { get; set; } =
	// 	new()
	// 	{
	// 		{ Guid.NewGuid().ToString(), new() },
	// 		{ Guid.NewGuid().ToString(), new() },
	// 		{ Guid.NewGuid().ToString(), new() },
	// 	};

	// [TomlNonSerialized]
	// public Dictionary<string, PiperNodeModel> Nodes1 => Nodes.ToDictionary(n => n.Name, n => n);

	[TomlPrecedingComment("\nLinks\n")]
	[TomlProperty("link")]
	public List<PiperLinkModel> Links { get; set; } =
		[new() { Id = ShortId.Generate() }, new() { }, new() { }];

	public string Save()
	{
		return null;
	}
}

public class PgNode { }

public class PgEdge { }
