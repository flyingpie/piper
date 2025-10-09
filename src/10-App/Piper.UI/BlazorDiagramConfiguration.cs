using Blazor.Diagrams;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.PathGenerators;
using Blazor.Diagrams.Core.Routers;
using Blazor.Diagrams.Options;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Piper.Core;
using Piper.Core.Nodes;
using Piper.UI.Components.Nodes;
using BD = Blazor.Diagrams.Core.Geometry;

namespace Piper.UI;

public static class BlazorDiagramConfiguration
{
	public static IServiceCollection AddBlazorDiagram(this IServiceCollection services)
	{
		return services
			.AddSingleton(p => CreateDiagram(CreateGraph()));
	}

	public static PpGraph CreateGraph()
	{
		var listFilesNode = new PpListFilesNode()
		{
			Name = "Node 3",
			InPath = "/home/marco/Downloads",
			InPattern = "*.txt",
			Position = new(50, 200),
		};

		var readFilesNode = new PpReadFilesNode()
		{
			Name = "Node 3",
			InAttr = "path",
			InFiles = { Table = () => listFilesNode.OutFiles.Table() },
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

	public static BlazorDiagram CreateDiagram(PpGraph graph)
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
			},
			Zoom = { Enabled = true, },
		};

		var diagram = new BlazorDiagram(options);
		diagram.RegisterComponent<GenericNodeModel<PpListFilesNode>, GenericNodeView<PpListFilesNode>>();
		diagram.RegisterComponent<GenericNodeModel<PpReadFilesNode>, GenericNodeView<PpReadFilesNode>>();

		diagram.Nodes.Add();

		// var listFilesNode = diagram.Nodes.Add(
		// 	new GenericNodeModel<PpListFilesNode>(new()
		// 		{
		// 			Name = "Node 3",
		// 			InPath = "/home/marco/Downloads",
		// 			InPattern = "*.txt",
		// 		})
		// 	{
		// 		Position = new BD.Point(50, 200),
		// 	});
		//
		// var readFilesNode = diagram.Nodes.Add(
		// 	new GenericNodeModel<PpReadFilesNode>(new()
		// 	{
		// 		Name = "Node 3",
		// 		InAttr = "path",
		// 		InFiles = { Table = () => listFilesNode.Node.OutFiles.Table() },
		// 		MaxFileSize = 12345,
		// 	})
		// 	{
		// 		Position = new BD.Point(400, 200),
		// 	});

		foreach (var n in diagram.Nodes.OfType<GenericNodeModel>())
		{
			foreach (var p in n.Ports)
			{
				var t = p.GetNodeInput?.Invoke()?.Table?.Invoke();
				if (t == null)
				{
					continue;
				}

				foreach (var n2 in diagram.Nodes.OfType<GenericNodeModel>())
				{
					foreach (var p2 in n2.Ports)
					{
						var t2 = p2.GetNodeOutput?.Invoke()?.Table?.Invoke();
						if (t2 == null)
						{
							continue;
						}

						if (t == t2)
						{
							diagram.Links.Add(new LinkModel(p2, p));
						}
					}
				}
			}
		}

		// diagram.Refresh();

		// var json = JsonConvert.SerializeObject(diagram.Nodes);

		return diagram;
	}
}