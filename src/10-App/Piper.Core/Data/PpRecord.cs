using System.Runtime.CompilerServices;

namespace Piper.Core.Data;

/// <summary>
///
/// </summary>
[CollectionBuilder(typeof(PpRecord), nameof(Create))]
public class PpRecord
{
	public PpRecord() { }

	public PpRecord(Dictionary<string, PpField> fields)
	{
		Fields = Guard.Against.Null(fields);
	}

	public static PpRecord Create(ReadOnlySpan<PpField> fields) { }

	public IDictionary<string, PpField> Fields { get; set; } = new Dictionary<string, PpField>(StringComparer.OrdinalIgnoreCase);

	// public static PpRecord Create(params PpField[] fields) => null;

	// public static implicit operator PpRecord(Dictionary<string, PpField> dict) => new(dict);

	// public static implicit operator PpRecord((string name, PpField value)[] items) => new(items.ToDictionary(i => i.name, i => i.value));

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
