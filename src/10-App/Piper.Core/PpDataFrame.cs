namespace Piper.Core;

public class PpDataFrame
{
	public static readonly PpDataFrame Empty = new()
	{
		Records = [],
	};

	public List<PpRecord> Records { get; set; } = [];

	// public IEnumerable<string> FieldNames => [];

	public IEnumerable<string> FieldNames =>
		Records
			.SelectMany(r => r.Fields.Keys)
			.Distinct();

	// public override string ToString() => $"{Records.Count} records";
}