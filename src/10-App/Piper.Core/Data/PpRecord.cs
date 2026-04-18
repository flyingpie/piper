using System.Collections;
using System.Runtime.CompilerServices;

namespace Piper.Core.Data;

/// <summary>
///
/// </summary>
[CollectionBuilder(typeof(PpRecord), nameof(Create))]
public class PpRecord : IEnumerable<PpField>
{
	public PpRecord() { }

	public PpRecord(Dictionary<string, PpField> fields)
	{
		Fields = Guard.Against.Null(fields);
	}

	public PpRecord(ReadOnlySpan<PpField> fields)
	{
		_fields = new(fields.ToArray());
	}

	public static PpRecord Create(ReadOnlySpan<PpField> fields) => new(fields);

	public IDictionary<string, PpField> Fields { get; set; } = new Dictionary<string, PpField>(StringComparer.OrdinalIgnoreCase);

	public IReadOnlyCollection<PpField> Fields2 => _fields;

	private List<PpField> _fields = [];

	// public static PpRecord Create(params PpField[] fields) => null;

	// public static implicit operator PpRecord(Dictionary<string, PpField> dict) => new(dict);

	// public static implicit operator PpRecord((string name, PpField value)[] items) => new(items.ToDictionary(i => i.name, i => i.value));

	public static PpRecord From(PpRecord record)
	{
		var res = new PpRecord();
		res._fields.AddRange(record._fields);

		return res;
	}

	public PpRecord With(params IEnumerable<PpField> fields)
	{
		_fields.AddRange(fields);

		return this;
	}

	public IEnumerator<PpField> GetEnumerator() => _fields.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

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
