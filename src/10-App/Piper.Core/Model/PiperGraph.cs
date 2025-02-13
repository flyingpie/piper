using System.Text.Json.Serialization;
using Blazor.Diagrams;
using Piper.Core.Model;
using Tomlet.Attributes;

namespace Piper.Core;

public class PiperGraph
{
	[JsonIgnore]
	[TomlNonSerialized]
	public List<PiperNodeModel> Nodes { get; set; } =
		[
			new PiperNodeModel() { Name = "Node 1" },
			new PiperNodeModel() { Name = "Node 2" },
			new PiperNodeModel() { Name = "Node 3" },
		];

	// public HashSet<PiperNodeModel> Nodes { get; set; } = new HashSet<PiperNodeModel>();

	// public Dictionary<string, PiperNodeModel> Nodes2 { get; set; } =
	// 	new()
	// 	{
	// 		{ Guid.NewGuid().ToString(), new() },
	// 		{ Guid.NewGuid().ToString(), new() },
	// 		{ Guid.NewGuid().ToString(), new() },
	// 	};

	//	[Tomlet.Attributes.TomlNonSerialized]
	public Dictionary<string, PiperNodeModel> Nodes1 => Nodes.ToDictionary(n => n.Name, n => n);

	public string Save()
	{
		return null;
	}
}

public class PgNode { }

public class PgEdge { }
