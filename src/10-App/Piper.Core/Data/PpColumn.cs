namespace Piper.Core.Data;

public class PpColumn(string name, PpDataType ppDataType)
{
	public string Name { get; } = Guard.Against.NullOrWhiteSpace(name);

	public PpDataType PpDataType { get; } = ppDataType;
}