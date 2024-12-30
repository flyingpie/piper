using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using Blazor.Diagrams;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Piper.Core;

public class SaveLoadService
{
	public void Load(BlazorDiagram diagram)
	{
		var jsonTxt = File.ReadAllText("/home/marco/Downloads/piper1.json");
		var model = JsonConvert.DeserializeObject<PiperSave>(jsonTxt);

		diagram.Groups.Clear();
		diagram.Nodes.Clear();
		diagram.Links.Clear();

		model.ApplyTo(diagram);
	}

	public void Save(BlazorDiagram diagram)
	{
		var model = new PiperSave()
		{
			Nodes = diagram.Nodes.ToList(),
			Links = diagram.Links.Select(l => ToLink(l)).Where(l => l != null).ToList(),
		};

		var json = JsonConvert.SerializeObject(model, new JsonSerializerSettings()
		{
			Formatting = Formatting.Indented,
			PreserveReferencesHandling = PreserveReferencesHandling.All,
			ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
			TypeNameHandling = TypeNameHandling.All,
		});

		// var json = JsonSerializer.Serialize(model, new()
		// {
		//
		// });

		File.WriteAllText("/home/marco/Downloads/piper1.json", json);

		var dbg = 2;
	}

	public static PiperSaveLink? ToLink(BaseLinkModel link)
	{
		var srcPort = link.Source.Model as PortModel;
		var dstPort = link.Target.Model as PortModel;

		if (srcPort == null || dstPort == null)
		{
			return null;
		}

		if (srcPort.Parent == null || dstPort.Parent == null)
		{
			return null;
		}

		return new PiperSaveLink() { Src = srcPort.Parent, Dst = dstPort.Parent, };
	}
}

public class PiperSave
{
	public List<NodeModel> Nodes { get; set; }

	public List<PiperSaveLink> Links { get; set; }

	public void ApplyTo(BlazorDiagram diagram)
	{
		foreach (var node in Nodes)
		{
			// diagram.Nodes.Add(
		}
	}
}

public class PiperSaveLink
{
	public NodeModel Src { get; set; }

	public NodeModel Dst { get; set; }
}