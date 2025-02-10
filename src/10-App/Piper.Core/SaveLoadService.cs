using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Blazor.Diagrams;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using Piper.UI.Components.Nodes;
//using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Piper.Core;

public class PolymorphicTypeResolver : DefaultJsonTypeInfoResolver
{
	public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
	{
		JsonTypeInfo jsonTypeInfo = base.GetTypeInfo(type, options);

		Type basePointType = typeof(NodeModel);
		if (jsonTypeInfo.Type == basePointType)
		{
			jsonTypeInfo.PolymorphismOptions = new JsonPolymorphismOptions
			{
				TypeDiscriminatorPropertyName = "$point-type",
				IgnoreUnrecognizedTypeDiscriminators = true,
				UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
				DerivedTypes =
				{
					new JsonDerivedType(typeof(ListFilesNodeModel), "3d"),
					// new JsonDerivedType(typeof(FourDimensionalPoint), "4d")
				},
			};
		}

		return jsonTypeInfo;
	}
}

public class SaveLoadService
{
	private static readonly JsonSerializerOptions _options = new JsonSerializerOptions()
	{
		// Converters = { new OptInJsonConverterFactory() },
		// Converters = { new ObjectHandleJsonConverterFactory() },
		ReferenceHandler = ReferenceHandler.Preserve,
		// TypeInfoResolver = new PolymorphicTypeResolver(),
		TypeInfoResolver = new DefaultJsonTypeInfoResolver
		{
			Modifiers =
			{
				// Add an Action<JsonTypeInfo> modifier that sets up the polymorphism options for BaseType
				static typeInfo =>
				{
					Console.WriteLine($"KIND:{typeInfo.Kind} TYPE:{typeInfo.Type.FullName}");

					if (typeInfo.Type != typeof(NodeModel))
						return;

					typeInfo.PolymorphismOptions = new()
					{
						DerivedTypes =
						{
							// new JsonDerivedType(typeof(NodeModel), nameof(NodeModel)),
							new JsonDerivedType(typeof(ListFilesNodeModel), "p1"),
						},
					};
				},
				// Add other modifiers as required.
			},
		},
		WriteIndented = true,
	};

	public void Load(BlazorDiagram diagram)
	{
		var jsonTxt = File.ReadAllText("/home/marco/Downloads/piper1.json");

		// var model = JsonConvert.DeserializeObject<PiperSave>(jsonTxt);
		var model = JsonSerializer.Deserialize<PiperSave>(jsonTxt, _options);

		diagram.Groups.Clear();
		diagram.Nodes.Clear();
		diagram.Links.Clear();

		model.ApplyTo(diagram);
	}

	public void Save(BlazorDiagram diagram)
	{
		var model = new PiperSave()
		{
			Nodes = diagram.Nodes.ToList(),
			Links = diagram.Links.Select(l => ToLink(l)).Where(l => l != null).ToList(),
		};

		// var json = JsonConvert.SerializeObject(model, new JsonSerializerSettings()
		// {
		// 	Formatting = Formatting.Indented,
		// 	PreserveReferencesHandling = PreserveReferencesHandling.All,
		// 	ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
		// 	TypeNameHandling = TypeNameHandling.Auto,
		// });

		var json = JsonSerializer.Serialize(model, _options);

		// var json = JsonSerializer.Serialize(model, new()
		// {
		//
		// });

		File.WriteAllText("/home/marco/Downloads/piper1.json", json);

		var dbg = 2;
	}

	public static PiperSaveLink? ToLink(BaseLinkModel link)
	{
		var srcPort = link.Source.Model as PortModel;
		var dstPort = link.Target.Model as PortModel;

		if (srcPort == null || dstPort == null)
		{
			return null;
		}

		if (srcPort.Parent == null || dstPort.Parent == null)
		{
			return null;
		}

		return new PiperSaveLink() { Src = srcPort.Parent, Dst = dstPort.Parent };
	}
}

public class PiperSave
{
	[JsonInclude]
	public List<NodeModel>? Nodes { get; set; }

	[JsonInclude]
	public List<PiperSaveLink>? Links { get; set; }

	public void ApplyTo(BlazorDiagram diagram)
	{
		foreach (var node in Nodes)
		{
			// diagram.Nodes.Add(
		}
	}
}

public class PiperSaveLink
{
	[JsonInclude]
	public NodeModel? Src { get; set; }

	[JsonInclude]
	public NodeModel? Dst { get; set; }
}

public class ObjectHandle
{
	public string? ObjType { get; set; }

	public object? Obj { get; set; }
}
