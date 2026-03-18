using System.Collections;
using System.Xml.Linq;
using System.Xml.XPath;
using Piper.Core.Attributes;
using Piper.Core.Db;
using static Piper.Core.Data.PpDataType;
using static Piper.Core.Data.PpPortDirection;

namespace Piper.Core.Data.Modifiers;

public class PpXPathModifier : PpModifier
{
	// public PpXPathModifier()
	// {
	//
	// }

	public override string Name { get; set; } = "XPath";

	[PpParam("Source Field")]
	public string SrcFieldName { get; set; } = "path";

	[PpParam("Query")]
	public string Query { get; set; } = "//text()";

	[PpParam("Destination Field")]
	public string DstFieldName { get; set; } = "xpath_1";

	public override async Task ExecuteAsync(IPpTable source, CancellationToken ct = default)
	{
		Guard.Against.Null(source);

		var cols = source.Columns.ToList();
		cols.AddRange([new(PpString, DstFieldName)]);
		Table.Columns = cols;
		await Table.ClearAsync(ct);

		await using var appender = await Table.CreateAppenderAsync(ct);

		await foreach (var rec in source.QueryAllAsync())
		{
			try
			{
				if (!rec.Fields.TryGetValue(SrcFieldName, out var src))
				{
					appender.Add(CreateRecord(rec, $"No field named '{SrcFieldName}'"));
				}

				var doc = XElement.Parse(src.ValueAsString);
				var q = (IEnumerable)doc.XPathEvaluate(Query);
				// var q = doc.XPathSelectElement(Query);

				// rec.Fields[DstFieldName] = "sup!";

				foreach (var part in q)
				{
					appender.Add(CreateRecord(rec, part?.ToString() ?? string.Empty));
				}
			}
			catch (Exception ex)
			{
				appender.Add(CreateRecord(rec, $"Wups: {ex.Message}"));
			}
		}

		await Table.DoneAsync();

		// var sql = $"""
		// 	create or replace view {Table.Name} as
		// 		select		*
		// 		,			upper({SrcFieldName}) as {DstFieldName}
		// 		from		{source.Name}
		// 	""";
		//
		// await PpDb.Instance.LowLevel.ExecuteNonQueryAsync(sql, ct);
		// await PpDb.Instance.FetchTableAsync(Table, ct);
	}

	private PpRecord CreateRecord(PpRecord file, string val) =>
		new() { Fields = new Dictionary<string, PpField>(file.Fields, StringComparer.OrdinalIgnoreCase) { { DstFieldName, val } } };
}
