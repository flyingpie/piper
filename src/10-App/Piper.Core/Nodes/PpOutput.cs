namespace Piper.Core.Nodes;

[AttributeUsage(AttributeTargets.Property)]
public sealed class PpOutput(string name) : Attribute
{
	public string Name { get; } = Guard.Against.NullOrWhiteSpace(name);
}