using System.Drawing;
using System.Numerics;
using Blazor.Diagrams.Core.Models;
using shortid;
using Tomlet.Attributes;

namespace Piper.Core.Model;

[TomlDoNotInlineObject]
public class PiperNodeModel : NodeModel
{
	[TomlProperty("type")]
	public string? NodeType { get; set; } = "run_process";

	[TomlProperty("id")]
	public string Id { get; set; } = ShortId.Generate();

	[TomlProperty("name")]
	public string? Name { get; set; } = "Node name 1";

	[TomlProperty("descr")]
	public string? Description { get; set; } = "";

	// [TomlProperty("pos")]
	// public Point Position { get; set; }

	// [TomlNonSerialized]
	// public List<PiperPortModel> Ports { get; set; } =
	// 	[
	// 		new PiperPortModel(this) { Direction = PiperPortDirection.In, Name = "stdin" },
	// 		new PiperPortModel(this) { Direction = PiperPortDirection.In, Name = "include" },
	// 		new PiperIntPortModel(this)
	// 		{
	// 			Direction = PiperPortDirection.In,
	// 			Name = "limit",
	// 			Max = 42,
	// 		},
	// 		new PiperPortModel() { Direction = PiperPortDirection.Out, Name = "stdout" },
	// 		new PiperStringPortModel()
	// 		{
	// 			Direction = PiperPortDirection.Out,
	// 			Name = "stderr",
	// 			Delimiter = "0",
	// 		},
	// 		new PiperPortModel() { Direction = PiperPortDirection.Out, Name = "errors" },
	// 	];
	//
	// [TomlProperty("in")]
	// public Dictionary<string, PiperPortModel> TomlPortsIn
	// // public List<PiperPortModel> TomlPortsIn
	// {
	// 	get =>
	// 		Ports
	// 			.Where(p => p.Direction == PiperPortDirection.In)
	// 			.ToDictionary(p => p.Name, p => p);
	// 	// set;
	// }
	//
	// [TomlProperty("out")]
	// public Dictionary<string, PiperPortModel> TomlPortsOut
	// {
	// 	get =>
	// 		Ports
	// 			.Where(p => p.Direction == PiperPortDirection.Out)
	// 			.ToDictionary(p => p.Name, p => p);
	// 	// set;
	// }

	// public Dictionary<string, string> InPorts2 { get; set; } =
	// 	new()
	// 	{
	// 		{ "port1", shortid.ShortId.Generate() },
	// 		{ "port2", shortid.ShortId.Generate() },
	// 		{ "port3", shortid.ShortId.Generate() },
	// 	};

	// [TomlProperty("out")]
	// public List<PiperPortModel> OutPorts { get; set; } = [];
}

public class RunProcessPiperNodeModel : PiperNodeModel
{
	[TomlProperty("cmd")]
	public string Command { get; set; } = "cat";

	[TomlNonSerialized]
	public List<CmdArgument> Args { get; set; } = [new() { Arg = "~/Downloads/*.csv" }];

	[TomlProperty("args")]
	public Dictionary<int, CmdArgument> Args1
	{
		get => Args.Select((a, i) => (a, i)).ToDictionary(p => p.i, p => p.a);
		// set;
	}

	public class CmdArgument
	{
		[TomlProperty("is_enabled")]
		public bool IsEnabled { get; set; } = true;

		[TomlProperty("arg")]
		public string? Arg { get; set; }
	}
}
