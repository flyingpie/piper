using Blazor.Diagrams;
using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.PathGenerators;
using Blazor.Diagrams.Core.Routers;
using Blazor.Diagrams.Options;
using Microsoft.Extensions.DependencyInjection;
using Piper.Core;
using Piper.Core.Nodes;
using Piper.Core.Serialization;
using Piper.UI.Components.Nodes;
using System.IO;

namespace Piper.UI;

public static class BlazorDiagramConfiguration
{
	public static IServiceCollection AddBlazorDiagram(this IServiceCollection services)
	{
		var path = "/home/marco/Downloads/graph.json";

		// var graph1 = CreateGraph();
		// var jsonOut = SerializeGraphJson(graph1);
		// File.WriteAllText(path, jsonOut);

		// var desJsonStr = File.ReadAllText(path);
		// var graph = PpNodeSerializer.DeserializeGraph(desJsonStr);

		return services
			.AddSingleton(p => CreateDiagram());
	}

	public static PpGraph CreateGraph()
	{
		var listFilesNode = new PpListFilesNode()
		{
			Name = "Node 2",
			InPath = "/home/marco/Downloads",
			InPattern = "*.txt",
			Position = new(50, 200),
		};

		var readFilesNode = new PpReadFilesNode()
		{
			Name = "Node 3",
			InAttr = "path",
			InFiles = { Output = listFilesNode.OutFiles },
			MaxFileSize = 12345,
			Position = new(400, 200),
		};

		return new PpGraph()
		{
			Nodes =
			[
				listFilesNode,
				readFilesNode,
			],
		};
	}

	private static BlazorDiagram CreateDiagram()
	{
		var options = new BlazorDiagramOptions
		{
			AllowPanning = true,
			AllowMultiSelection = true,
			Links =
			{
				DefaultColor = "#ffffff",
				DefaultPathGenerator = new SmoothPathGenerator(),
				DefaultRouter = new NormalRouter(),
				EnableSnapping = true,
				SnappingRadius = 15,
				// Factory =
			},
			Zoom = { Enabled = true, },
		};

		var diagram = new BlazorDiagram(options);
		diagram.RegisterComponent<PpListFilesNode, GenericNodeView>();
		diagram.RegisterComponent<PpReadFilesNode, GenericNodeView>();

		// diagram.LoadGraph(graph);

		return diagram;
	}
}

// public class PpLink : LinkModel
// {
// 	public PpLink(Anchor source, Anchor target) : base(source, target)
// 	{
// 	}
//
// 	public PpLink(string id, Anchor source, Anchor target) : base(id, source, target)
// 	{
// 	}
//
// 	public PpLink(PortModel sourcePort, PortModel targetPort) : base(sourcePort, targetPort)
// 	{
// 	}
//
// 	public PpLink(NodeModel sourceNode, NodeModel targetNode) : base(sourceNode, targetNode)
// 	{
// 	}
//
// 	public PpLink(string id, PortModel sourcePort, PortModel targetPort) : base(id, sourcePort, targetPort)
// 	{
// 	}
//
// 	public PpLink(string id, NodeModel sourceNode, NodeModel targetNode) : base(id, sourceNode, targetNode)
// 	{
// 	}
// }