using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Piper.Core.Serialization;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Piper.Core.Utils;

public static class PpJson
{
	private static JsonSerializerOptions JsonOpts = new()
	{
		Converters =
		{
			new JsonStringEnumConverter(),
			new PpJsonNodeIdJsonConverter(),
			new PpJsonNodeParamJsonConverter(),
			new PpJsonPortLinkJsonConverter(),
			new Vector2JsonConverter(),
		},
		IncludeFields = true,
		PropertyNamingPolicy = JsonNamingPolicy.KebabCaseLower,
		ReferenceHandler = ReferenceHandler.IgnoreCycles,
		TypeInfoResolver = new DefaultJsonTypeInfoResolver(),
		DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,

		IndentCharacter = '\t',
		IndentSize = 1,
		WriteIndented = true,
	};

	public static T? Deserialize<T>(string json) => JsonSerializer.Deserialize<T>(json, JsonOpts);

	public static T DeserializeRequired<T>(string json) =>
		Deserialize<T>(json) ?? throw new InvalidOperationException($"Json '{json}' deserialized to null.");

	public static string SerializeToString(object? obj) => JsonSerializer.Serialize(obj, JsonOpts);
}

public static class PpYaml
{
	private static readonly ISerializer _serializer = new SerializerBuilder()
		.WithNamingConvention(new UnderscoredNamingConvention())
		.Build();

	public static string SerializeToString(object? obj) => _serializer.Serialize(obj);
}
