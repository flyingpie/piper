namespace Piper.Core.Nodes;

[AttributeUsage(AttributeTargets.Property)]
public sealed class PpPortAttribute(PpPortDirection direction) : Attribute
{
	public PpPortDirection Direction { get; } = direction;
}