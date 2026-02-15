namespace Piper.Core.Data;

public class PpColumn(PpDataType dataType, string name)
{
	public PpDataType DataType { get; } = dataType;

	public string Name { get; } = Guard.Against.NullOrWhiteSpace(name);
}
