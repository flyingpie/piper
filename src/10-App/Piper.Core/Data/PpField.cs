namespace Piper.Core.Data;

public class PpField
{
	public PpField() { }

	public PpField(PpDataType type, object? value)
	{
		DataType = Guard.Against.Null(type);
		Value = value;
	}

	public PpDataType DataType { get; set; }

	public object? Value { get; set; }

	// public bool? ValueAsBool { get; set; }

	// public DateTime? ValueAsDateTime => Value as DateTime?;

	// public Guid? ValueAsGuid { get; set; }

	// public long? ValueAsLong { get; set; }

	public string? ValueAsString => Value as string;

	// public object? Value2 => ValueAsLong?.ToString() ?? ValueAsString;

	public static implicit operator PpField(bool? valueAsBool) => new(PpDataType.PpBool, valueAsBool);

	public static implicit operator PpField(int? valueAsInt) => new(PpDataType.PpInt32, valueAsInt);

	public static implicit operator PpField(long? valueAsLong) => new(PpDataType.PpInt64, valueAsLong);

	public static implicit operator PpField(string? str) => new(PpDataType.PpString, str);

	public static implicit operator PpField(string[] str) => new(PpDataType.PpStringArray, str);

	public override string ToString() => Value?.ToString() ?? "(empty)";
}
