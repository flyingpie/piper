using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Piper.Utils;

public static class PpYaml
{
	private static readonly ISerializer _serializer = new SerializerBuilder()
		.WithNamingConvention(new UnderscoredNamingConvention())
		.Build();

	public static string SerializeToString(object? obj) => _serializer.Serialize(obj);
}
