using System.Text.Json;
using System.Text.Json.Serialization;

namespace Piper.Core.Serialization;

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