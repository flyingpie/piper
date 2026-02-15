namespace Piper.Core.Data;

/// <summary>
///
/// </summary>
public class PpRecord
{
	public IDictionary<string, PpField> Fields { get; set; } = new Dictionary<string, PpField>(StringComparer.OrdinalIgnoreCase);

	public override string ToString() => string.Join(", ", Fields);

	public object? GetValue(string name)
	{
		if (!Fields.TryGetValue(name, out var field))
		{
			return null;
		}

		return field.ToString();
	}
}
