using System.Text.Json;
using System.Text.Json.Serialization;

namespace Piper.Core.Serialization;

public class PpNodeParamJsonConverter : JsonConverter<PpJsonParam>
{
	public override PpJsonParam? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var str = reader.GetString();
		var spl = str.Split(':', 2);
		var type = spl[0];
		var val = spl[1];

		switch (type.ToLowerInvariant())
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