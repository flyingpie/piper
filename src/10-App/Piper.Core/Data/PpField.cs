using Piper.Core.Utils;

namespace Piper.Core.Data;

public class PpField
{
	private PpDataTypeFlags? _flags;
	private List<PpField>? _listOfFields;

	public PpField(PpDataType type, object? value)
	{
		DataType = Guard.Against.Null(type);
		Value = value;
	}

	public PpField(string name, PpDataType type, object? value)
	{
		Name = name;
		DataType = Guard.Against.Null(type);
		Value = value;
	}

	public string Name { get; set; }

	public PpDataType DataType { get; }

	public PpDataTypeFlags DataTypeFlags => _flags ??= GetFlags();

	public object? Value { get; }

	public string? ValueAsString => Value as string ?? Value?.ToString();

	public List<PpField>? ValueAsList
	{
		get
		{
			if (_listOfFields is not null)
			{
				return _listOfFields;
			}

			if (Value is List<string> listOfStrings)
			{
				_listOfFields = listOfStrings.Select(s => new PpField(PpDataType.PpString, s)).ToList();
				return _listOfFields;
			}

			return null;
		}
	}

	public static implicit operator PpField((string Name, PpDataType Type, object? Value) f) => new(f.Name, f.Type, f.Value);

	public static implicit operator PpField((string Name, string? Value) f) => new(f.Name, PpDataType.PpString, f.Value);

	public static implicit operator PpField((string Name, bool? Value) f) => new(f.Name, PpDataType.PpBool, f.Value);

	//

	public static implicit operator PpField(bool? valueAsBool) => new(PpDataType.PpBool, valueAsBool);

	public static implicit operator PpField(decimal? valueAsInt) => new(PpDataType.PpDouble, valueAsInt);

	public static implicit operator PpField(double? valueAsInt) => new(PpDataType.PpDouble, valueAsInt);

	public static implicit operator PpField(int? valueAsInt) => new(PpDataType.PpInt32, valueAsInt);

	public static implicit operator PpField(long? valueAsLong) => new(PpDataType.PpInt64, valueAsLong);

	public static implicit operator PpField(string? str) => new(PpDataType.PpString, str);

	public static implicit operator PpField(string[] str) => new(PpDataType.PpStringArray, str);

	public PpColumn AsColumn() => new PpColumn(DataType, Name);

	// public override string ToString() => Value?.ToString() ?? "(empty)";
	public override string ToString() => $"({Name}, \"{Value}\"::{DataType})";

	private static readonly Regex _hexRegex = new("^#([A-Fa-f0-9]{3}|[A-Fa-f0-9]{6}|[A-Fa-f0-9]{8})$", RegexOptions.Compiled);

	private PpDataTypeFlags GetFlags()
	{
		// Empty
		if (Value is null || string.IsNullOrWhiteSpace(ValueAsString))
		{
			return PpDataTypeFlags.None;
		}

		// Hex color
		if (ValueAsString is not null && _hexRegex.IsMatch(ValueAsString))
		{
			return PpDataTypeFlags.HexColor;
		}

		// JSON
		if (ValueAsString is not null && PpJson.LooksLikeJson(ValueAsString))
		{
			return PpDataTypeFlags.Json;
		}

		// XML
		if (ValueAsString is not null && PpXml.LooksLikeXml(ValueAsString))
		{
			return PpDataTypeFlags.Xml;
		}

		// Multi-Line
		if (ValueAsString is not null && ValueAsString.Contains('\n'))
		{
			return PpDataTypeFlags.MultiLine;
		}

		return PpDataTypeFlags.None;
	}
}
