namespace Piper.Core.Serialization;

public class PpJsonPortLink(string node, string port)
{
	public string Node { get; } = Guard.Against.NullOrWhiteSpace(node);

	public string Port { get; } = Guard.Against.NullOrWhiteSpace(port);
}
