using Blazor.Diagrams.Core.Geometry;
using Piper.Core.Attributes;
using Piper.Core.Data;
using Piper.Core.Nodes;
using System.Globalization;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using YamlDotNet.Serialization;

namespace Piper.Core.Serialization;

public static class NodeSerializer
{
	private static JsonSerializerOptions jsonOpts = new()
	{
		Converters =
		{
			new PpJsonNodeIdJsonConverter(),
			new PpJsonParamJsonConverter(),
			new PpJsonPortJsonConverter(),
			new Vector2JsonConverter(),
		},
		IncludeFields = true,
		PropertyNamingPolicy = JsonNamingPolicy.KebabCaseLower,
		ReferenceHandler = ReferenceHandler.IgnoreCycles,
		TypeInfoResolver = new DefaultJsonTypeInfoResolver(),
		DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,

		IndentCharacter = '\t',
		IndentSize = 1,
		WriteIndented = true,
	};

	public static string SerializeGraphYaml(PpGraph graph)
	{
		var json = SerializeGraphJson(graph);

		var j = Newtonsoft.Json.JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JArray>(json);
		var s = new SerializerBuilder()
			.Build();

		var yaml = s.Serialize(j);

		return null;
	}

	public static string SerializeGraphJson(PpGraph graph)
	{
		var jsonGraph = SerializeGraph(graph);
		var json = JsonSerializer.Serialize(jsonGraph, jsonOpts);

		return json;
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
						// jsonNode.Params[prop.Name] = new PpJsonParam() { IntValue = vInt };
						jsonNode.Params ??= new();
						jsonNode.Params[prop.Name] = vInt.ToString(CultureInfo.InvariantCulture);
					}
					if (v is string vStr)
					{
						// jsonNode.Params[prop.Name] = new PpJsonParam() { StrValue = vStr };
						jsonNode.Params ??= new();
						jsonNode.Params[prop.Name] = vStr;
					}
				}

				var attrPort = attrs.OfType<PpPortAttribute>().FirstOrDefault();
				if (attrPort != null)
				{
					if (attrPort.Direction == PpPortDirection.In)
					{
						// jsonNode.Ports[prop.Name] = null;

						if (prop.GetValue(n) is PpNodeInput { Output.Node: not null } inPort)
						{
							jsonNode.Ports ??= new();
							jsonNode.Ports[prop.Name] = new(inPort.Output.Node.NodeId, inPort.Output.Name);
						}
					}
					else
					{
						// jsonNode.Ports[prop.Name] = null;

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
			if (!nodeTypes.TryGetValue(n.Id.Type, out var nodeType))
			{
				Console.WriteLine($"No such node type '{n.Id.Type}'");
				continue;
			}

			if (Activator.CreateInstance(nodeType) is not PpNodeBase nodeInst)
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

public class Vector2JsonConverter : JsonConverter<Vector2>
{
	public override Vector2 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var str = reader.GetString();
		var spl = str.Split(',');
		var x = float.Parse(spl[0]);
		var y = float.Parse(spl[1]);

		return new(x, y);
	}

	public override void Write(Utf8JsonWriter writer, Vector2 value, JsonSerializerOptions options)
	{
		writer.WriteStringValue($"{(int)value.X},{(int)value.Y}");
	}
}

public class PpJsonPortJsonConverter : JsonConverter<PpJsonPort>
{
	public override PpJsonPort? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var str = reader.GetString();
		var spl = str.Split(':');
		var nodeId = spl[0];
		var portName = spl[1];

		return new PpJsonPort(nodeId, portName);
	}

	public override void Write(Utf8JsonWriter writer, PpJsonPort value, JsonSerializerOptions options)
	{
		writer.WriteStringValue($"{value.Node}:{value.Port}");
	}
}

public class PpJsonParamJsonConverter : JsonConverter<PpJsonParam>
{
	public override PpJsonParam? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var str = reader.GetString();
		var spl = str.Split(':', 2);
		var type = spl[0];
		var val = spl[1];

		switch(type.ToLowerInvariant())
		{
			case "int":
				return new PpJsonParam() { IntValue = int.Parse(val) };
			case "str":
				return new PpJsonParam() { StrValue = val };
			default:
				throw new InvalidOperationException($"Invalid param type '{type}'.");
		}
	}

	public override void Write(Utf8JsonWriter writer, PpJsonParam value, JsonSerializerOptions options)
	{
		if (value.IntValue != null)
		{
			writer.WriteStringValue($"int:{value.IntValue}");
		}

		if (value.StrValue != null)
		{
			writer.WriteStringValue($"str:{value.StrValue}");
		}
	}
}

public class PpJsonNodeIdJsonConverter : JsonConverter<PpJsonNodeId>
{
	public override PpJsonNodeId? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var str = reader.GetString();
		var spl = str.Split(':', 3);
		var id = spl[0];
		var type = spl[1];
		var name = spl[2];

		return new(id, type, name);
	}

	public override void Write(Utf8JsonWriter writer, PpJsonNodeId value, JsonSerializerOptions options)
	{
		writer.WriteStringValue($"{value.Id}:{value.Type}:{value.Name}");
	}
}