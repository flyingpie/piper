using Blazor.Diagrams;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.PathGenerators;
using Blazor.Diagrams.Core.Routers;
using Blazor.Diagrams.Options;
using Microsoft.Extensions.DependencyInjection;
using Piper.Core;
using Piper.Core.Attributes;
using Piper.Core.Data;
using Piper.Core.Nodes;
using Piper.UI.Components.Nodes;
using System.IO;
using System.Numerics;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Piper.UI;

public static class BlazorDiagramConfiguration
{
	private static JsonSerializerOptions jsonOpts = new JsonSerializerOptions()
	{
		ReferenceHandler = ReferenceHandler.IgnoreCycles,
		PropertyNamingPolicy = JsonNamingPolicy.KebabCaseLower,
		TypeInfoResolver = new DefaultJsonTypeInfoResolver(),
		WriteIndented = true,
		IndentCharacter = '\t',
		IndentSize = 1,
	};

	public static IServiceCollection AddBlazorDiagram(this IServiceCollection services)
	{
		var path = "/home/marco/Downloads/graph.json";

		// var graph1 = CreateGraph();
		// var jsonOut = SerializeGraphJson(graph1);
		// File.WriteAllText(path, jsonOut);

		var desJsonStr = File.ReadAllText(path);
		var graph = DeserializeGraph(desJsonStr);

		return services
			.AddSingleton(p => CreateDiagram(graph));
	}

	private static string SerializeGraphJson(PpGraph graph)
	{
		var jsonGraph = SerializeGraph(graph);
		var json = JsonSerializer.Serialize(jsonGraph, jsonOpts);

		return json;
	}

	private static List<PpJsonNode> SerializeGraph(PpGraph graph)
	{
		var obj = new List<PpJsonNode>();

		foreach (var n in graph.Nodes)
		{
			var jsonNode = new PpJsonNode()
			{
				Name = n.Name,
				Type = n.GetType().Name,
				Pos = new(n.Position.X, n.Position.Y),
			};

			foreach (var prop in n.GetType().GetProperties())
			{
				var attrs = prop.GetCustomAttributes(inherit: true);

				var attrParam = attrs.OfType<PpParamAttribute>().FirstOrDefault();
				if (attrParam != null)
				{
					var v = prop.GetValue(n);
					if (v is int vInt)
					{
						jsonNode.Params[prop.Name] = new PpJsonParam() { IntValue = vInt };
					}
					if (v is string vStr)
					{
						jsonNode.Params[prop.Name] = new PpJsonParam() { StrValue = vStr };
					}
				}

				var attrPort = attrs.OfType<PpPortAttribute>().FirstOrDefault();
				if (attrPort != null)
				{
					if (attrPort.Direction == PpPortDirection.In)
					{
						jsonNode.Ports[prop.Name] = null;

						if (prop.GetValue(n) is PpNodeInput { Output.Node: not null } inPort)
						{
							jsonNode.Ports[prop.Name] = new(inPort.Output.Node.Name, inPort.Output.Name);
						}
					}
					else
					{
						jsonNode.Ports[prop.Name] = null;

						// if(prop.GetValue(n) is PpNodeOutput {}
						// jsonNode[prop.Name] = prop.GetValue(n);
					}
				}
			}

			obj.Add(jsonNode);
		}

		return obj;
	}

	public static PpGraph DeserializeGraph(string json)
	{
		var nodes = JsonSerializer.Deserialize<List<PpJsonNode>>(json, jsonOpts);

		return DeserializeGraph(nodes);
	}

	public static PpGraph DeserializeGraph(List<PpJsonNode> nodes)
	{
		var graph = new PpGraph();

		var nodeTypes = typeof(PpNodeBase) // TODO: Pull from all loaded assemblies
			.Assembly
			.GetTypes()
			.Where(t => !t.IsAbstract)
			.Where(t => t.IsAssignableTo(typeof(PpNodeBase)))
			.ToDictionary(t => t.Name, t => t, StringComparer.OrdinalIgnoreCase);

		foreach (var n in nodes)
		{
			if (!nodeTypes.TryGetValue(n.Type, out var nodeType))
			{
				Console.WriteLine($"No such node type '{n.Type}'");
				continue;
			}

			if (Activator.CreateInstance(nodeType) is not PpNodeBase nodeInst)
			{
				Console.WriteLine($"Cannot instantiate node of type '{nodeType.FullName}'");
				continue;
			}

			nodeInst.Name = n.Name;
			nodeInst.Position = new Point(n.Pos.X, n.Pos.Y);

			// Params
			foreach (var p in n.Params)
			{
				var prop = nodeType.GetProperty(p.Key);
				if (prop == null)
				{
					Console.WriteLine($"Could not get property with name '{p.Key}' on type '{nodeType.FullName}'");
					continue;
				}

				if (prop.PropertyType == typeof(string))
				{
					prop.SetValue(nodeInst, p.Value.StrValue);
				}

				if (prop.PropertyType == typeof(int))
				{
					prop.SetValue(nodeInst, p.Value.IntValue);
				}
			}

			graph.Nodes.Add(nodeInst);
		}

		foreach (var jsonNode in nodes)
		{
			var inNode = graph.Nodes.FirstOrDefault(x => x.Name == jsonNode.Name);

			foreach (var port in jsonNode.Ports)
			{
				if (port.Value == null)
				{
					continue;
				}

				var inPort = inNode.GetType().GetProperty(port.Key);
				if (inPort == null)
				{
					continue;
				}

				if (inPort.GetValue(inNode) is not PpNodeInput nodeInput)
				{
					continue;
				}

				var outNode = graph.Nodes.FirstOrDefault(nx => nx.Name == port.Value.Node);
				if (outNode == null)
				{
					continue;
				}

				var outPort = outNode.GetType().GetProperty(port.Value.Port);
				if (outPort == null)
				{
					continue;
				}

				if (outPort.GetValue(outNode) is not PpNodeOutput nodeOutput)
				{
					continue;
				}

				nodeInput.Output = nodeOutput;
			}
		}

		return graph;
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