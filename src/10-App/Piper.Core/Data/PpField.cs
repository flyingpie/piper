namespace Piper.Core.Data;

public class PpField
{
	public PpField() { }

	public PpField(bool? valueAsBool)
	{
		ValueAsBool = valueAsBool;
	}

	public PpField(Guid? valueAsGuid)
	{
		ValueAsGuid = valueAsGuid;
	}

	public PpField(int? valueAsLong)
	{
		ValueAsLong = valueAsLong;
	}

	public PpField(long? valueAsLong)
	{
		ValueAsLong = valueAsLong;
	}

	public PpField(string? valueAsString)
	{
		// ValueAsString = valueAsString;
	}

	public PpField(PpDataType type, object? value)
	{
		DataType = Guard.Against.Null(type);
		Value = value;
	}

	// [JsonIgnore]
	public PpDataType DataType { get; set; }

	public object? Value { get; set; }

	public bool? ValueAsBool { get; set; }

	public DateTime? ValueAsDateTime => Value as DateTime?;

	public Guid? ValueAsGuid { get; set; }

	public long? ValueAsLong { get; set; }

	public string? ValueAsString => Value as string;

	public object? Value2 => ValueAsLong?.ToString() ?? ValueAsString;

	public static implicit operator PpField(bool? valueAsBool) => new(valueAsBool);

	public static implicit operator PpField(int? valueAsInt) => new(valueAsInt);

	public static implicit operator PpField(long? valueAsLong) => new(valueAsLong);

	public static implicit operator PpField(string str) => new(PpDataType.PpString, str);

	public static implicit operator PpField(string[] str) => new(PpDataType.PpStringArray, str);

	public override string ToString() => Value?.ToString() ?? "(empty)";
}
