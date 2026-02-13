namespace Piper.Core.Serialization;

public class PpJsonPort
{
	public PpJsonPortLink Link { get; set; }

	public List<PpJsonModifier> Mods { get; set; }
}

public class PpJsonPortLink(string node, string port)
{
	public string Node { get; } = Guard.Against.NullOrWhiteSpace(node);

	public string Port { get; } = Guard.Against.NullOrWhiteSpace(port);
}

public class PpJsonModifier
{
	public string Type { get; set; }
}
