using System.Text.Json.Serialization;

namespace Piper.Core.Data;

public class PpField
{
	public PpField()
	{
	}

	public PpField(Guid? valueAsGuid)
	{
		ValueAsGuid = valueAsGuid;
	}

	public PpField(int? valueAsInt)
	{
		ValueAsInt = valueAsInt;
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

	public bool ValueAsBool
	{
		get;
		set;
	}

	public DateTime? ValueAsDateTime => Value as DateTime?;

	public Guid? ValueAsGuid { get; set; }

	public int? ValueAsInt { get; set; }

	public string? ValueAsString => Value as string;

	public object? Value2 => ValueAsInt?.ToString() ?? ValueAsString;

	public static implicit operator PpField(int? valueAsInt) => new(valueAsInt);

	public static implicit operator PpField(string str) => new(PpDataType.PpString, str);

	public override string ToString() => Value?.ToString() ?? "(empty)";
}