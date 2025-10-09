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
		ValueAsString = valueAsString;
	}

	public Guid? ValueAsGuid { get; set; }

	public int? ValueAsInt { get; set; }

	public string? ValueAsString { get; set; }

	public object? Value => ValueAsInt?.ToString() ?? ValueAsString;

	public static implicit operator PpField(int? valueAsInt) => new(valueAsInt);

	public static implicit operator PpField(string str) => new(str);

	public override string ToString() => Value?.ToString() ?? "(empty)";
}