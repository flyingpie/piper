using System.Text.Json;
using System.Text.Json.Serialization;

namespace Piper.Core.Serialization;

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