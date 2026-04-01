using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Piper.Core.Serialization;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Piper.Core.Utils;

public static class PpJson
{
	private static readonly JsonSerializerOptions _jsonOpts = new()
	{
		Converters =
		{
			new JsonStringEnumConverter(),
			new PpJsonNodeIdJsonConverter(),
			new PpJsonNodeTypeAndNameJsonConverter(),
			new PpJsonNodeParamJsonConverter(),
			new PpJsonPortLinkJsonConverter(),
			new Vector2JsonConverter(),
		},
		DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
		Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, // Causes UTF8 characters to be serialized as-is, instead of (less readable) escape codes.
		IncludeFields = true,
		IndentCharacter = '\t',
		IndentSize = 1,
		PropertyNamingPolicy = JsonNamingPolicy.KebabCaseLower,
		ReferenceHandler = ReferenceHandler.IgnoreCycles,
		TypeInfoResolver = new DefaultJsonTypeInfoResolver(),
		WriteIndented = true,
	};

	public static T Deserialize<T>(string json) => JsonSerializer.Deserialize<T>(json, _jsonOpts)!;

	public static T DeserializeRequired<T>(string json) =>
		Deserialize<T>(json) ?? throw new InvalidOperationException($"Json '{json}' deserialized to null.");

	public static bool TryDeserialize<T>(string json, out T? result)
	{
		result = default;

		// Json
		try
		{
			result = Deserialize<T>(json);
			return true;
		}
		catch { }

		return false;
	}

	public static bool LooksLikeJson(string str) => (str?.StartsWith('[') ?? false) || (str?.StartsWith('{') ?? false);

	public static string SerializeToString(object? obj) => JsonSerializer.Serialize(obj, _jsonOpts);

	// public static JsonNode SerializeToElement(object value)
	// {
	// 	JsonNode.Parse();
	// 	// return JsonSerializer.SerializeToElement(value, _jsonOpts);
	// }
}

public static class PpYaml
{
	private static readonly ISerializer _serializer = new SerializerBuilder()
		.WithNamingConvention(new UnderscoredNamingConvention())
		.Build();

	public static string SerializeToString(object? obj) => _serializer.Serialize(obj);
}

public static class PpXml
{
	public static bool LooksLikeXml(string str) => str?.StartsWith('<') ?? false;
}
