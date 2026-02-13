using System.Text.Json;
using System.Text.Json.Serialization;

namespace Piper.Core.Serialization;

public class PpJsonPortLinkJsonConverter : JsonConverter<PpJsonPortLink>
{
	public override PpJsonPortLink? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var str = reader.GetString();
		var spl = str.Split(':');
		var nodeId = spl[0];
		var portName = spl[1];

		return new PpJsonPortLink(nodeId, portName);
	}

	public override void Write(Utf8JsonWriter writer, PpJsonPortLink value, JsonSerializerOptions options)
	{
		writer.WriteStringValue($"{value.Node}:{value.Port}");
	}
}
