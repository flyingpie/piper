using System.Globalization;
using System.Reflection;
using System.Text.Json.Nodes;
using Blazor.Diagrams.Core.Geometry;
using Piper.Core.Attributes;
using Piper.Core.Data;
using Piper.Core.Data.Modifiers;
using Piper.Core.Utils;

namespace Piper.Core.Serialization;

public static class PpNodeSerializer
{
	public static string SerializeGraphJson(PpGraph graph)
	{
		var jsonGraph = SerializeGraph(graph);

		return PpJson.SerializeToString(jsonGraph);
		// return PpYaml.SerializeToString(jsonGraph);
	}

	// public static JsonNode SerializeGraphNode(PpGraph graph)
	// {
	// 	var jsonGraph = SerializeGraph(graph);
	//
	// 	return PpJson.SerializeToString(jsonGraph);
	// 	// return PpYaml.SerializeToString(jsonGraph);
	// }

	public static List<PpJsonNode> SerializeGraph(PpGraph graph)
	{
		var obj = new List<PpJsonNode>();

		foreach (var n in graph.Nodes)
		{
			var jsonNode = new PpJsonNode()
			{
				Id = new(n.NodeId, n.GetType().Name, n.Name),
				Pos = new((float)n.Position.X, (float)n.Position.Y),
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
						jsonNode.Params ??= new();
						jsonNode.Params[prop.Name] = vInt.ToString(CultureInfo.InvariantCulture);
					}

					if (v is string vStr)
					{
						jsonNode.Params ??= new();
						jsonNode.Params[prop.Name] = vStr;
					}
				}

				var attrPort = attrs.OfType<PpPortAttribute>().FirstOrDefault();
				if (attrPort != null)
				{
					if (prop.GetValue(n) is Piper.Core.Data.PpNodePort nodePort)
					{
						SerializeNodePort(nodePort, jsonNode, prop, n);
					}

					// if (attrPort.Direction == PpPortDirection.In)
					// {
					// 	if (prop.GetValue(n) is PpNodeInput { Output.Node: not null } inPort)
					// 	{
					// 		jsonNode.Ports ??= new();
					// 		jsonNode.Ports[prop.Name] = new()
					// 		{
					// 			//
					// 			Link = new(inPort.Output.Node.NodeId, inPort.Output.Name),
					// 			Mods = [],
					// 		};
					// 	}
					// }
				}
			}

			obj.Add(jsonNode);
		}

		return obj;
	}

	private static void SerializeNodePort(Data.PpNodePort nodePort, PpJsonNode jsonNode, PropertyInfo prop, PpNode n)
	{
		var port = new PpJsonPort();
		foreach (var mod in nodePort.Modifiers.Modifiers)
		{
			port.Mods ??= [];

			var jsonMod = SerializeModifier(mod);

			port.Mods.Add(jsonMod);
		}

		// Port modifiers
		// nodePort.Modifiers

		jsonNode.Ports ??= new();
		jsonNode.Ports[prop.Name] = port;

		if (prop.GetValue(n) is PpNodeInput { Output.Node: not null } inPort)
		{
			port.Link = new(inPort.Output.Node.NodeId, inPort.Output.Name);
		}
	}

	private static PpJsonModifier SerializeModifier(PpModifier mod)
	{
		var jsonMod = new PpJsonModifier() { Id = new(id: mod.Id, type: mod.GetType().Name, name: mod.Name) };

		foreach (var modProp in mod.GetType().GetProperties())
		{
			var attrs = modProp.GetCustomAttributes(inherit: true);

			var attrParam = attrs.OfType<PpParamAttribute>().FirstOrDefault();

			if (attrParam != null)
			{
				var v = modProp.GetValue(mod);
				if (v is int vInt)
				{
					jsonMod.Params ??= new();
					jsonMod.Params[modProp.Name] = vInt.ToString(CultureInfo.InvariantCulture);
				}

				if (v is string vStr)
				{
					jsonMod.Params ??= new();
					jsonMod.Params[modProp.Name] = vStr;
				}
			}
		}

		return jsonMod;
	}

	public static PpGraph DeserializeGraph(string json)
	{
		var nodes = PpJson.DeserializeRequired<List<PpJsonNode>>(json);

		return DeserializeGraph(nodes);
	}

	public static PpGraph DeserializeGraph(List<PpJsonNode> nodes)
	{
		var graph = new PpGraph();

		var nodeTypes = typeof(PpNode) // TODO: Pull from all loaded assemblies
			.Assembly.GetTypes()
			.Where(t => !t.IsAbstract)
			.Where(t => t.IsAssignableTo(typeof(PpNode)))
			.ToDictionary(t => t.Name, t => t, StringComparer.OrdinalIgnoreCase);

		foreach (var n in nodes)
		{
			if (!nodeTypes.TryGetValue(n.Id.Type, out var nodeType))
			{
				Console.WriteLine($"No such node type '{n.Id.Type}'");
				continue;
			}

			if (Activator.CreateInstance(nodeType) is not PpNode nodeInst)
			{
				Console.WriteLine($"Cannot instantiate node of type '{nodeType.FullName}'");
				continue;
			}

			nodeInst.NodeId = n.Id.Id;
			nodeInst.Name = n.Id.Name;
			nodeInst.Position = new Point(n.Pos.X, n.Pos.Y);

			// Params
			foreach (var p in n.Params ?? [])
			{
				var prop = nodeType.GetProperty(p.Key);
				if (prop == null)
				{
					Console.WriteLine($"Could not get property with name '{p.Key}' on type '{nodeType.FullName}'");
					continue;
				}

				if (prop.PropertyType == typeof(string))
				{
					prop.SetValue(nodeInst, p.Value);
				}

				if (prop.PropertyType == typeof(int))
				{
					prop.SetValue(nodeInst, int.Parse(p.Value));
				}
			}

			graph.Nodes.Add(nodeInst);
		}

		DeserializeModifiers(graph, nodes);

		foreach (var jsonNode in nodes)
		{
			var inNode = graph.Nodes.FirstOrDefault(x => x.NodeId == jsonNode.Id.Id);

			foreach (var port in jsonNode.Ports ?? [])
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

				var outNode = graph.Nodes.FirstOrDefault(nx => nx.NodeId == port.Value.Link.Node);
				if (outNode == null)
				{
					continue;
				}

				var outPort = outNode.GetType().GetProperty(port.Value.Link.Port);
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

	private static void DeserializeModifiers(PpGraph graph, List<PpJsonNode> nodes)
	{
		var modTypes = typeof(PpModifier) // TODO: Pull from all loaded assemblies
			.Assembly.GetTypes()
			.Where(t => !t.IsAbstract)
			.Where(t => t.IsAssignableTo(typeof(PpModifier)))
			.ToDictionary(t => t.Name, t => t, StringComparer.OrdinalIgnoreCase);

		foreach (var jsonNode in nodes)
		{
			var ppNode = graph.Nodes.FirstOrDefault(x => x.NodeId == jsonNode.Id.Id);

			foreach (var jsonPort in jsonNode.Ports ?? [])
			{
				// var ppNodePort = (Data.PpNodePort)ppNode.NodeProps.FirstOrDefault(p => p.Name == jsonPort.Key);
				var ppNodePort = (Data.PpNodePort)ppNode.GetType().GetProperty(jsonPort.Key).GetValue(ppNode);

				foreach (var jsonMod in jsonPort.Value.Mods ?? [])
				{
					// Modifiers
					if (!modTypes.TryGetValue(jsonMod.Id.Type, out var modType))
					{
						Console.WriteLine($"No such modifier type '{jsonMod.Id.Type}'");
						continue;
					}

					if (Activator.CreateInstance(modType) is not PpModifier modInst)
					{
						Console.WriteLine($"Cannot instantiate node of type '{modType.FullName}'");
						continue;
					}

					modInst.Id = jsonMod.Id.Id;
					modInst.Name = jsonMod.Id.Name;
					// modInst.Position = new Point(n.Pos.X, n.Pos.Y);

					// Params
					foreach (var p in jsonMod.Params ?? [])
					{
						var prop = modType.GetProperty(p.Key);
						if (prop == null)
						{
							Console.WriteLine($"Could not get property with name '{p.Key}' on type '{modType.FullName}'");
							continue;
						}

						if (prop.PropertyType == typeof(string))
						{
							prop.SetValue(modInst, p.Value);
						}

						if (prop.PropertyType == typeof(int))
						{
							prop.SetValue(modInst, int.Parse(p.Value));
						}
					}

					ppNodePort.Modifiers.Add(modInst);
				}
			}
		}
	}
}
