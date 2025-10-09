using Blazor.Diagrams;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.PathGenerators;
using Blazor.Diagrams.Core.Routers;
using Blazor.Diagrams.Options;
using Microsoft.Extensions.DependencyInjection;
using Piper.Core.Attributes;
using Piper.Core.Data;
using Piper.Core.Nodes;
using Piper.UI.Components.Nodes;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Piper.UI;

public class PpJsonNode : Dictionary<string, object?>
{
	// public string NodeType { get; set; }
}

public static class BlazorDiagramConfiguration
{
	public static IServiceCollection AddBlazorDiagram(this IServiceCollection services)
	{
		var graph = CreateGraph();

		var obj = new List<PpJsonNode>();

		foreach (var n in graph.Nodes)
		{
			var jsonNode = new PpJsonNode()
			{
				// NodeType = n.NodeType,
			};

			foreach (var prop in n.GetType().GetProperties())
			{
				var attrs = prop.GetCustomAttributes(inherit: true);

				var attrParam = attrs.OfType<PpParamAttribute>().FirstOrDefault();
				if (attrParam != null)
				{
					jsonNode[prop.Name] = prop.GetValue(n);
				}

				var attrPort = attrs.OfType<PpPortAttribute>().FirstOrDefault();
				if (attrPort != null)
				{
					if (attrPort.Direction == PpPortDirection.In)
					{
						jsonNode[prop.Name] = null;

						if (prop.GetValue(n) is PpNodeInput { Output.Node: not null } inPort)
						{
							jsonNode[prop.Name] = $"{inPort.Output.Node.Name}:{inPort.Output.Name}";
						}
					}
					else
					{
						jsonNode[prop.Name] = null;

						// if(prop.GetValue(n) is PpNodeOutput {}
						// jsonNode[prop.Name] = prop.GetValue(n);
					}
				}
			}

			obj.Add(jsonNode);
		}

		var json = JsonSerializer.Serialize(obj, new JsonSerializerOptions()
		{
			ReferenceHandler = ReferenceHandler.IgnoreCycles,
			PropertyNamingPolicy = JsonNamingPolicy.KebabCaseLower,
			// DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault | JsonIgnoreCondition.WhenWritingNull,
			TypeInfoResolver = new DefaultJsonTypeInfoResolver(),
			WriteIndented = true,
			IndentCharacter = '\t',
			IndentSize = 1,
		});

		return services
			.AddSingleton(p => CreateDiagram(graph));
	}

	public static PpGraph CreateGraph()
	{
		var listFilesNode = new PpListFilesNode()
		{
			Name = "Node 3", InPath = "/home/marco/Downloads", InPattern = "*.txt", Position = new(50, 200),
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
		diagram.RegisterComponent<PpListFilesNode, GenericNodeView>();
		diagram.RegisterComponent<PpReadFilesNode, GenericNodeView>();

		graph.Nodes.ForEach(n => diagram.Nodes.Add(n));

		// var listFilesNode = diagram.Nodes.Add(
		// 	new PpListFilesNode()
		// 		{
		// 			Name = "Node 3",
		// 			InPath = "/home/marco/Downloads",
		// 			InPattern = "*.txt",
		// 			Position = new BD.Point(50, 200),
		// 		});
		//
		// var readFilesNode = diagram.Nodes.Add(
		// 	new PpReadFilesNode()
		// 	{
		// 		Name = "Node 3",
		// 		InAttr = "path",
		// 		InFiles = { Table = () => listFilesNode.OutFiles.Table() },
		// 		MaxFileSize = 12345,
		// 		Position = new BD.Point(400, 200),
		// 	});

		foreach (var n in diagram.Nodes.OfType<PpNodeBase>())
		{
			foreach (var p in n.Ports)
			{
				var t = p.GetNodeInput?.Invoke()?.Output;
				if (t == null)
				{
					continue;
				}

				foreach (var n2 in diagram.Nodes.OfType<PpNodeBase>())
				{
					foreach (var p2 in n2.Ports)
					{
						var t2 = p2.GetNodeOutput?.Invoke();
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