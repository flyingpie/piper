using System.Text.Json;
using System.Xml.Linq;
using Piper.Core.Utils;

namespace Piper.Core.Data;

public class PpField(PpDataType type, object? value)
{
	private PpDataTypeFlags? _flags;

	public PpDataType DataType { get; } = Guard.Against.Null(type);

	public PpDataTypeFlags DataTypeFlags => _flags ??= GetFlags();

	public object? Value { get; } = value;

	public string? ValueAsString => Value as string;

	public static implicit operator PpField(bool? valueAsBool) => new(PpDataType.PpBool, valueAsBool);

	public static implicit operator PpField(int? valueAsInt) => new(PpDataType.PpInt32, valueAsInt);

	public static implicit operator PpField(long? valueAsLong) => new(PpDataType.PpInt64, valueAsLong);

	public static implicit operator PpField(string? str) => new(PpDataType.PpString, str);

	public static implicit operator PpField(string[] str) => new(PpDataType.PpStringArray, str);

	public override string ToString() => Value?.ToString() ?? "(empty)";

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