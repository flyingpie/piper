using Blazor.Diagrams.Core.Geometry;
using Piper.Core.Attributes;
using Piper.Core.Data;
using Piper.Core.Utils;
using System.Globalization;

namespace Piper.Core.Serialization;

public static class PpNodeSerializer
{
	public static string SerializeGraphJson(PpGraph graph)
	{
		var jsonGraph = SerializeGraph(graph);

		return PpJson.SerializeToString(jsonGraph);
	}

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
					if (attrPort.Direction == PpPortDirection.In)
					{
						if (prop.GetValue(n) is PpNodeInput { Output.Node: not null } inPort)
						{
							jsonNode.Ports ??= new();
							jsonNode.Ports[prop.Name] = new(inPort.Output.Node.NodeId, inPort.Output.Name);
						}
					}
				}
			}

			obj.Add(jsonNode);
		}

		return obj;
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
			.Assembly
			.GetTypes()
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

				var outNode = graph.Nodes.FirstOrDefault(nx => nx.NodeId == port.Value.Node);
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
}