using System.Dynamic;
using Piper.Core.Attributes;
using RazorEngineCore;
using static Piper.Core.Data.PpDataType;

namespace Piper.Core.Data.Modifiers;

public class PpRazorModifier : PpModifier
{
	public override string Name { get; set; } = "Razor";

	// [PpParam("Source Field")]
	// public string? SrcFieldName { get; set; }

	[PpParam("Destination Field")]
	public string? DstFieldName { get; set; }

	[PpParam("Template", Hint = PpParamHint.Code)]
	public string? Template { get; set; }

	public override async Task ExecuteAsync(IPpTable source, CancellationToken ct = default)
	{
		Guard.Against.Null(source);

		// if (string.IsNullOrWhiteSpace(SrcFieldName))
		// {
		// 	return;
		// }

		if (string.IsNullOrWhiteSpace(DstFieldName))
		{
			return;
		}

		if (string.IsNullOrWhiteSpace(Template))
		{
			return;
		}

		var cols = source.Columns.ToList();
		cols.AddRange([new(PpString, DstFieldName)]);
		Table.Columns = cols;
		await Table.ClearAsync(ct);

		var engine = new RazorEngine();
		var tpl = await engine.CompileAsync<CustomRazorTemplate>(Template, cancellationToken: ct);

		await using var appender = await Table.CreateAppenderAsync(ct);

		await foreach (var rec in source.QueryAllAsync(ct))
		{
			try
			{
				// if (!rec.Fields.TryGetValue(SrcFieldName, out var src))
				// {
				// 	appender.Add(CreateRecord(rec, $"No field named '{SrcFieldName}'"));
				// 	break;
				// }

				// var doc = XElement.Parse(src.ValueAsString);
				// var q = (IEnumerable)doc.XPathEvaluate(Query);
				// var b = new StringBuilder();
				// if (q is string)
				// {

				var eo = new ExpandoObject();
				var eoColl = (ICollection<KeyValuePair<string, object>>)eo;
				foreach (var kv in rec.Fields)
				{
					eoColl.Add(new KeyValuePair<string, object>(kv.Key, kv.Value.ValueAsString ?? string.Empty));
				}

				var res = tpl.Run(inst => inst.Rec = eo);

				var newRec = new PpRecord()
				{
					//
					Fields = new Dictionary<string, PpField>(rec.Fields, StringComparer.OrdinalIgnoreCase),
				};
				newRec.Fields.Add(DstFieldName, res);
				appender.Add(newRec);
			}
			catch (Exception ex) { }
		}

		await Table.DoneAsync(ct);
	}

	private PpRecord CreateRecord(PpRecord file, string val) =>
		new() { Fields = new Dictionary<string, PpField>(file.Fields, StringComparer.OrdinalIgnoreCase) { { DstFieldName, val } } };

	public class CustomRazorTemplate : RazorEngineTemplateBase
	{
		public dynamic Rec { get; set; }
	}
}
