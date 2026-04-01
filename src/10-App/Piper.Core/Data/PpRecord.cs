namespace Piper.Core.Data;

/// <summary>
///
/// </summary>
public class PpRecord
{
	public PpRecord()
	{
	}

	// public PpRecord(Dictionary<string, PpField> fields)
	// {
	// 	Fields = Guard.Against.Null(fields);
	// }

	public IDictionary<string, PpField> Fields { get; set; } = new Dictionary<string, PpField>(StringComparer.OrdinalIgnoreCase);

	// public static PpRecord Create(params PpField[] fields) => null;

	public override string ToString() => string.Join(", ", Fields);

	public PpField? GetField(string name)
	{
		if (!Fields.TryGetValue(name, out var field))
		{
			return null;
		}

		return field;
	}

	public object? GetValue(string name)
	{
		if (!Fields.TryGetValue(name, out var field))
		{
			return null;
		}

		return field.ToString();
	}
}
