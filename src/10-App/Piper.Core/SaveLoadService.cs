using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Blazor.Diagrams;
using Blazor.Diagrams.Core.Models;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Piper.Core;

public class SaveLoadService
{
	public void Load(BlazorDiagram diagram)
	{

	}

	public void Save(BlazorDiagram diagram)
	{
		var model = new PiperSave()
		{
			Nodes = diagram.Nodes.ToList(),
		};

		var json = JsonConvert.SerializeObject(model, new JsonSerializerSettings()
		{
			PreserveReferencesHandling = PreserveReferencesHandling.All,
			ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
		});

		// var json = JsonSerializer.Serialize(model, new()
		// {
		//
		// });

		var dbg = 2;
	}
}

public class PiperSave
{
	public List<NodeModel> Nodes { get; set; }

	// public List<> Type { get; set; }
}