using System.Collections;
using System.Xml.Linq;
using System.Xml.XPath;
using Piper.Core.Attributes;
using static Piper.Core.Data.PpDataType;

namespace Piper.Core.Data.Modifiers;

public class PpXPathModifier : PpModifier
{
	public override string Name { get; set; } = "XPath";

	[PpParam("Source Field")]
	public string SrcFieldName { get; set; } = "path";

	[PpParam("Query")]
	public string Query { get; set; } = "//text()";

	[PpParam("Destination Field")]
	public string DstFieldName { get; set; } = "xpath_1";

	// [PpParam("Queries")]
	// public List<XPathQuery> Queries { get; set; } = [];

	public override async Task ExecuteAsync(IPpTable source, CancellationToken ct = default)
	{
		Guard.Against.Null(source);

		var cols = source.Columns.ToList();
		cols.AddRange([new(PpString, DstFieldName)]);
		// cols.AddRange(Queries.Select(q => new PpColumn(PpString, q.DstFieldName)));
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
					break;
				}

				var doc = XElement.Parse(src.ValueAsString);
				var q = (IEnumerable)doc.XPathEvaluate(Query);
				var b = new StringBuilder();
				if (q is string)
				{
					var newRec = new PpRecord() { Fields = new Dictionary<string, PpField>(rec.Fields, StringComparer.OrdinalIgnoreCase) };
					newRec.Fields.Add(DstFieldName, q.ToString() ?? string.Empty);
					appender.Add(newRec);
				}
				else
				{
					foreach (var xx in q)
					{
						var newRec = new PpRecord() { Fields = new Dictionary<string, PpField>(rec.Fields, StringComparer.OrdinalIgnoreCase) };
						// b.Append(xx?.ToString() ?? string.Empty);
						newRec.Fields.Add(DstFieldName, xx?.ToString() ?? string.Empty);
						appender.Add(newRec);
					}
				}

				// foreach (var query in Queries)
				// {
				// 	if (!rec.Fields.TryGetValue(query.SrcFieldName, out var src))
				// 	{
				// 		// appender.Add(CreateRecord(rec, $"No field named '{query.SrcFieldName}'"));
				// 		break;
				// 	}
				// 	var doc = XElement.Parse(src.ValueAsString);
				// 	var q = (IEnumerable)doc.XPathEvaluate(query.Query);
				// 	var b = new StringBuilder();
				// 	foreach (var xx in q)
				// 	{
				// 		b.Append(xx?.ToString() ?? string.Empty);
				// 	}
				//
				// 	// foreach (var part in q)
				// 	// {
				// 	// 	appender.Add(CreateRecord(rec, part?.ToString() ?? string.Empty));
				// 	// }
				//
				// 	newRec.Fields.Add(query.DstFieldName, b.ToString());
				// }

				// appender.Add(newRec);
			}
			catch (Exception ex)
			{
				// var errRec = new PpRecord()
				// {
				// 	Fields = new Dictionary<string, PpField>(rec.Fields, StringComparer.OrdinalIgnoreCase)
				// 	{
				// 		{ "err", $"Wups: {ex.Message}" }
				// 	}
				// };
				//
				// appender.Add(errRec);
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

	public class XPathQuery
	{
		[PpParam("Source Field")]
		public string SrcFieldName { get; set; } = "path";

		[PpParam("Query")]
		public string Query { get; set; } = "//text()";

		[PpParam("Destination Field")]
		public string DstFieldName { get; set; } = "xpath_1";
	}
}
