// using System.Runtime.Serialization;
//
// namespace Piper.Core.Data;
//
// [DataContract(Name = "Reverse", Namespace = "PpModifier")]
// public class PpReverseModifier : PpModifier
// {
// 	public override string Name { get; set; } = "Reverse";
//
// 	public string FieldName { get; set; } = "rec__uuid";
//
// 	public override Task<PpRecord> ExecuteAsync(PpRecord record)
// 	{
// 		// record.Fields["new_field"] = "A string";
//
// 		if (FieldName == null)
// 		{
// 			return Task.FromResult(record);
// 		}
//
// 		record.Fields["new_field"] = string.Empty;
// 		if (record.Fields.TryGetValue(FieldName, out var field))
// 		{
// 			record.Fields["new_field"] = new string(field.Value?.ToString()?.Reverse()?.ToArray() ?? []);
// 		}
//
// 		return Task.FromResult(record);
// 	}
// }
