using System.Text.Json;
using System.Text.Json.Serialization;

namespace Piper.Core.Serialization;

public class PpJsonNodeTypeAndNameJsonConverter : JsonConverter<PpJsonNodeTypeAndName>
{
	public override PpJsonNodeTypeAndName? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var str = reader.GetString();
		var spl = str.Split(':', 2);
		var type = spl[0];
		var name = spl[1];

		return new(type, name);
	}

	public override void Write(Utf8JsonWriter writer, PpJsonNodeTypeAndName value, JsonSerializerOptions options)
	{
		writer.WriteStringValue($"{value.Type}:{value.Name}");
	}
}
