using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Piper.Core.Serialization;

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
