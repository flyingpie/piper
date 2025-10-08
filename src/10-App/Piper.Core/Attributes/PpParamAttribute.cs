namespace Piper.Core.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public sealed class PpParamAttribute(string name) : Attribute
{
	public string Name { get; } = Guard.Against.NullOrWhiteSpace(name);
}