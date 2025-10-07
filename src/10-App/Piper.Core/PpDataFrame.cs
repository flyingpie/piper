// namespace Piper.Core;
//
// public class PpDataFrame
// {
// 	public string TableName { get; set; }
//
// 	// public string Name { get; } = Guard.Against.NullOrWhiteSpace(name);
//
// 	public static readonly PpDataFrame Empty = new()
// 	{
// 		Records = [],
// 	};
//
// 	public List<PpRecord> Records { get; set; } = [];
//
// 	// public IEnumerable<string> FieldNames => [];
//
// 	public IEnumerable<string> FieldNames =>
// 		Records
// 			.SelectMany(r => r.Fields.Keys)
// 			.Distinct();
//
// 	// public override string ToString() => $"{Records.Count} records";
// }