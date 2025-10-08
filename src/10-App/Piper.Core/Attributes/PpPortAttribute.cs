using Piper.Core.Data;

namespace Piper.Core.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public sealed class PpPortAttribute(PpPortDirection direction, string name) : Attribute
{
	public string Name { get; } = Guard.Against.NullOrWhiteSpace(name);

	public PpPortDirection Direction { get; } = direction;
}