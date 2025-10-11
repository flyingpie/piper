using Blazor.Diagrams;
using Microsoft.AspNetCore.Components;
using Piper.Core;
using Piper.Core.Nodes;
using Piper.Core.Serialization;
using Radzen;
using System.IO;
using BD = Blazor.Diagrams.Core.Geometry;

namespace Piper.UI.Components;

public partial class PpDiagram : ComponentBase
{
	private int _idx;

	[Inject]
	private BlazorDiagram? Diagram { get; set; }

	public List<PpGraphFile> Graphs { get; set; } = [];
	// [
	// 	new() { Name = "My graph 1" },
	// 	new() { Name = "Some other graph" },
	// 	new() { Name = "Convert to csv" },
	// 	new() { Name = "Format some stuff" },
	// ];

	protected override async Task OnInitializedAsync()
	{
		// var pp = Path.GetDirectoryName(new Uri(GetType().Assembly.Location).AbsolutePath);
		var dir = "/home/marco/workspace/flyingpie/piper_1/graphs";

		Graphs = Directory
			.GetFiles(dir, "*.json")
			.Select(p => new PpGraphFile() { Path = p })
			.ToList();
	}

	private void OnMenuItemClick(MenuItemEventArgs args)
	{
		Console.WriteLine($"VALUE:{args.Value}");

		switch (args.Value ?? string.Empty)
		{
			case "PP_NODE_LIST_FILES":
				Diagram.Nodes.Add(
					new PpListFilesNode()
					{
						Name = $"List Files {++_idx:00}",
						InPath = "/home/marco/Downloads",
						InPattern = "*.txt",
						Position = new BD.Point(args.ClientX, args.ClientY),
					});
				break;

			case "PP_NODE_READ_FILES":
				Diagram.Nodes.Add(
					new PpReadFilesNode()
					{
						Name = $"Read Files {++_idx:00}",
						InAttr = "path",
						Position = new BD.Point(args.ClientX, args.ClientY),
					});
				// var node = Diagram.Nodes.Add(
				// 	new PpReadFilesNode()
				// 	{
				// 		Name = "Node 3",
				// 		Position = new BD.Point(args.ClientX, args.ClientY),
				// 	});
				break;

			default:
				Console.WriteLine($"Unknown context value '{args.Value}'");
				break;
		}

		ContextMenuService.Close();
	}

	private void LoadGraphFile(PpGraphFile file)
	{
		var graph = NodeSerializer.DeserializeGraph(File.ReadAllText(file.Path));

		Diagram.LoadGraph(graph);
	}

	private void SaveGraphFile(PpGraphFile file)
	{
		var graph = NodeSerializer.SerializeGraphJson(Diagram.GetGraph());

		File.WriteAllText(file.Path, graph);
	}
}

public class PpGraphFile
{
	public string Name => System.IO.Path.GetFileName(Path);

	public string Path { get; set; }
}