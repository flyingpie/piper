using Piper.Core.Utils;

namespace Piper.Core.Data;

public class PpRecord
{
	public IDictionary<string, PpField> Fields { get; set; } = new Dictionary<string, PpField>(StringComparer.OrdinalIgnoreCase);

	public static PpRecord FromJson(string json) =>
		new PpRecord() { Fields = PpJson.DeserializeRequired<IDictionary<string, PpField>>(json) };

	public string AsJson() => PpJson.SerializeToString(Fields);

	public override string ToString() => string.Join(", ", Fields);
}
