using System.IO;
using Blazor.Diagrams;
using Microsoft.AspNetCore.Components;
using Piper.Core;
using Piper.Core.Serialization;

namespace Piper.UI.Components;

public partial class PpGraphsPanel : ComponentBase
{
	[Inject]
	private BlazorDiagram? Diagram { get; set; }

	public List<PpGraphFile> Graphs { get; set; } = [];

	protected override async Task OnInitializedAsync()
	{
		var dir = "/home/marco/workspace/flyingpie/piper_1/graphs";

		Graphs = Directory.GetFiles(dir, "*.json").Select(p => new PpGraphFile() { Path = p }).ToList();
	}

	private void LoadGraphFile(PpGraphFile file)
	{
		var graph = PpNodeSerializer.DeserializeGraph(File.ReadAllText(file.Path));

		Diagram.LoadGraph(graph);
	}

	private void SaveGraphFile(PpGraphFile file)
	{
		var graph = PpNodeSerializer.SerializeGraphJson(Diagram.GetGraph());

		File.WriteAllText(file.Path, graph);
	}
}
