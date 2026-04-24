using System.Dynamic;
using Piper.Core.Attributes;
using RazorEngineCore;

namespace Piper.Core.Data.Modifiers;

public class PpRazorModifier : PpModifier
{
	private readonly IRazorEngine _engine = new RazorEngine();

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

		Table.Clear();

		var tpl = await CompileTemplateAsync(ct);
		if (tpl == null)
		{
			return;
		}

		await using var appender = await Table.CreateAppenderAsync(ct);

		await foreach (var rec in source.QueryAllAsync(ct))
		{
			var newRec = PpRecord.From(rec);

			try
			{
				// Turn current record into template parameter.
				var tplRec = CreateTemplateParams(rec);

				// Execute template.
				var res = await tpl.RunAsync(inst => inst.Rec = tplRec);

				// Add result field.
				newRec.With((DstFieldName, res));
			}
			catch (Exception ex)
			{
				newRec.With((DstFieldName, $"Error running Razor template: {ex.Message}"));
			}

			appender.Add(newRec);
		}

		await Table.DoneAsync(ct);
	}

	private static ExpandoObject CreateTemplateParams(PpRecord rec)
	{
		var eo = new ExpandoObject();
		ICollection<KeyValuePair<string, object?>> eoColl = eo;
		foreach (var kv in rec.Fields2)
		{
			eoColl.Add(new KeyValuePair<string, object?>(kv.Name, kv.Value));
		}

		return eo;
	}

	private async Task<IRazorEngineCompiledTemplate<CustomRazorTemplate>?> CompileTemplateAsync(CancellationToken ct)
	{
		IRazorEngineCompiledTemplate<CustomRazorTemplate>? tpl;

		try
		{
			return await _engine.CompileAsync<CustomRazorTemplate>(Template, cancellationToken: ct);
		}
		catch (Exception ex)
		{
			Logs.Error($"Error compiling Razor template: {ex.Message}");
			return null;
		}
	}

	private class CustomRazorTemplate : RazorEngineTemplateBase
	{
		public dynamic Rec { get; set; } = null!;
	}
}
