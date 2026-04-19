using System.Dynamic;
using Piper.Core.Attributes;
using RazorEngineCore;
using static Piper.Core.Data.PpDataType;

namespace Piper.Core.Data.Modifiers;

public class PpRazorModifier : PpModifier
{
	public override string Name { get; set; } = "Razor";

	[PpParam("Destination Field")]
	public string? DstFieldName { get; set; }

	[PpParam("Template", Hint = PpParamHint.Code)]
	public string? Template { get; set; }

	public override async Task ExecuteAsync(IPpTable source, CancellationToken ct = default)
	{
		Guard.Against.Null(source);

		if (string.IsNullOrWhiteSpace(DstFieldName))
		{
			return;
		}

		if (string.IsNullOrWhiteSpace(Template))
		{
			return;
		}

		// var cols = source.Columns.ToList();
		// cols.AddRange([new(PpString, DstFieldName)]);
		// Table.Columns = cols;
		// await Table.ClearAsync(ct);
		Table.Clear();

		var engine = new RazorEngine();
		IRazorEngineCompiledTemplate<CustomRazorTemplate>? tpl;

		try
		{
			tpl = await engine.CompileAsync<CustomRazorTemplate>(Template, cancellationToken: ct);
		}
		catch (Exception ex)
		{
			Logs.Error($"Error compiling Razor template: {ex.Message}");
			return;
		}

		await using var appender = await Table.CreateAppenderAsync(ct);

		await foreach (var rec in source.QueryAllAsync(ct))
		{
			// var newRec = new PpRecord()
			// {
			// 	//
			// 	Fields = new Dictionary<string, PpField>(rec.Fields, StringComparer.OrdinalIgnoreCase),
			// };
			var newRec = PpRecord.From(rec);

			try
			{
				var eo = new ExpandoObject();
				ICollection<KeyValuePair<string, object?>> eoColl = eo;
				foreach (var kv in rec.Fields2)
				{
					eoColl.Add(new KeyValuePair<string, object?>(kv.Name, kv.Value));
				}

				var res = tpl.Run(inst => inst.Rec = eo);

				// newRec.Fields.Add(DstFieldName, res);
				newRec.With((DstFieldName, res));
			}
			catch (Exception ex)
			{
				// newRec.Fields.Add(DstFieldName, $"Error running Razor template: {ex.Message}");
				newRec.With((DstFieldName, $"Error running Razor template: {ex.Message}"));
			}

			appender.Add(newRec);
		}

		await Table.DoneAsync(ct);
	}

	// private PpRecord CreateRecord(PpRecord file, string val) =>
	// 	new() { Fields = new Dictionary<string, PpField>(file.Fields, StringComparer.OrdinalIgnoreCase) { { DstFieldName, val } } };

	public class CustomRazorTemplate : RazorEngineTemplateBase
	{
		public dynamic Rec { get; set; }
	}
}
