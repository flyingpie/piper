using Blazor.Diagrams;
using Newtonsoft.Json;

namespace Piper.Core;

public class SaveLoadService
{
	// private static readonly JsonSerializerOptions JsonOptions = new()
	// {
	// 	ReferenceHandler = ReferenceHandler.Preserve,
	// 	WriteIndented = true,
	// };

	public PiperGraph Load()
	{
		var jsonTxt = File.ReadAllText("/home/marco/Downloads/piper1.json");

		// var model = JsonSerializer.Deserialize<PiperSave>(jsonTxt, JsonOptions);

		// diagram.Groups.Clear();
		// diagram.Nodes.Clear();
		// diagram.Links.Clear();

		// model.ApplyTo(diagram);

		return null;
	}

	public string Write(PiperGraph diagram)
	{
		// var rootNode = new PiperSaveNode();

		// var model = new PiperSave()
		// {
		// 	Nodes = diagram.Nodes.OfType<PiperNodeModel>().ToList(),
		// 	Links = diagram.Links.OfType<PiperLinkModel>().Where(l => l != null).ToList(),
		// };

		return JsonConvert.SerializeObject(diagram, new JsonSerializerSettings()
		{

		});

		// return JsonSerializer.Serialize(diagram, JsonOptions);
	}

	public void Save(PiperGraph diagram)
	{
		var json = Write(diagram);

		File.WriteAllText("/home/marco/Downloads/piper1.json", json);

		var dbg = 2;
	}

	// public static PiperSaveLink? ToLink(BaseLinkModel link)
	// {
	// 	var srcPort = link.Source.Model as PortModel;
	// 	var dstPort = link.Target.Model as PortModel;
	//
	// 	if (srcPort == null || dstPort == null)
	// 	{
	// 		return null;
	// 	}
	//
	// 	if (srcPort.Parent == null || dstPort.Parent == null)
	// 	{
	// 		return null;
	// 	}
	//
	// 	return new PiperSaveLink() { SrcId = srcPort.Parent, DstId = dstPort.Parent };
	// }
}

// public class PiperSave
// {
// 	[JsonInclude]
// 	public List<PiperNodeModel>? Nodes { get; set; }
//
// 	[JsonInclude]
// 	public List<PiperLinkModel>? Links { get; set; }
//
// 	public void ApplyTo(BlazorDiagram diagram)
// 	{
// 		foreach (var node in Nodes)
// 		{
// 			// diagram.Nodes.Add(
// 		}
// 	}
// }

// public class PiperSaveLink
// {
// 	public Guid Id { get; set; }
//
// 	[JsonInclude]
// 	public Guid? SrcId { get; set; }
//
// 	[JsonInclude]
// 	public Guid? DstId { get; set; }
// }

// public class PiperSaveNode : Dictionary<string, object>
// {
// 	public PiperSaveNode()
// 		: base(StringComparer.OrdinalIgnoreCase) { }
//
// 	// public Guid Id { get; set; }
//
// 	// public Vector2 Position { get; set; }
//
// 	// public Guid Id
// 	// {
// 	// 	get => GetAs<Guid>(nameof(Id));
// 	// 	set => Set(nameof(Id), value);
// 	// }
//
// 	// public Guid SomeOtherId
// 	// {
// 	// 	get => GetAs<Guid>(nameof(SomeOtherId));
// 	// 	set => Set(nameof(SomeOtherId), value);
// 	// }
//
// 	public T? GetAs<T>(string key)
// 	{
// 		var val = this[key];
//
// 		return (T)val;
// 	}
//
// 	public void Set(string key, object val)
// 	{
// 		this[key] = val;
// 	}
// }
//
// public class ObjectHandle
// {
// 	public string? ObjType { get; set; }
//
// 	public object? Obj { get; set; }
// }
