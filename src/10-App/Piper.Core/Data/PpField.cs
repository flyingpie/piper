namespace Piper.Core.Data;

public class PpField(PpDataType type, object? value)
{
	public PpDataType DataType { get; } = Guard.Against.Null(type);

	public object? Value { get; } = value;

	public string? ValueAsString => Value as string;

	public static implicit operator PpField(bool? valueAsBool) => new(PpDataType.PpBool, valueAsBool);

	public static implicit operator PpField(int? valueAsInt) => new(PpDataType.PpInt32, valueAsInt);

	public static implicit operator PpField(long? valueAsLong) => new(PpDataType.PpInt64, valueAsLong);

	public static implicit operator PpField(string? str) => new(PpDataType.PpString, str);

	public static implicit operator PpField(string[] str) => new(PpDataType.PpStringArray, str);

	public override string ToString() => Value?.ToString() ?? "(empty)";
}
