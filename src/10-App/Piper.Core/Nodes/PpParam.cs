namespace Piper.Core.Nodes;

[AttributeUsage(AttributeTargets.Property)]
public sealed class PpParam(string name) : Attribute
{
	public string Name { get; } = Guard.Against.NullOrWhiteSpace(name);
}