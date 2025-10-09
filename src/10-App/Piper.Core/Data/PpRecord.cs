namespace Piper.Core.Data;

public class PpRecord
{
	public IDictionary<string, PpField> Fields { get; set; } = new Dictionary<string, PpField>(StringComparer.OrdinalIgnoreCase);

	public override string ToString() => string.Join(", ", Fields);
}