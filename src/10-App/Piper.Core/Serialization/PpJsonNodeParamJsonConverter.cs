using System.Text.Json;
using System.Text.Json.Serialization;

namespace Piper.Core.Serialization;

public class PpJsonNodeParamJsonConverter : JsonConverter<PpJsonNodeParam>
{
	public override PpJsonNodeParam? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var str = reader.GetString();
		var spl = str.Split(':', 2);
		var type = spl[0];
		var val = spl[1];

		switch (type.ToLowerInvariant())
		{
			case "int":
				return new PpJsonNodeParam() { IntValue = int.Parse(val) };
			case "str":
				return new PpJsonNodeParam() { StrValue = val };
			default:
				throw new InvalidOperationException($"Invalid param type '{type}'.");
		}
	}

	public override void Write(Utf8JsonWriter writer, PpJsonNodeParam value, JsonSerializerOptions options)
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
