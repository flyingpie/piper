using Microsoft.Extensions.Logging;
using Piper.Core.Attributes;
using Piper.Core.Utils;
using static Piper.Core.Data.PpDataType;

namespace Piper.Core.Data.Modifiers;

public class PpReadFilesModifier : PpModifier
{
	private readonly ILogger _log = Log.For<PpReadFilesModifier>();

	public override string Name { get; set; } = "Read Files";

	[PpParam("File Name Field")]
	public string? SrcFieldName { get; set; }

	[PpParam("Destination Field")]
	public string? DstFieldName { get; set; }

	// [PpParam("Template", Hint = PpParamHint.Code)]
	// public string? Template { get; set; }

	public override async Task ExecuteAsync(IPpTable source, CancellationToken ct = default)
	{
		Guard.Against.Null(source);

		if (string.IsNullOrWhiteSpace(SrcFieldName))
		{
			return;
		}

		if (string.IsNullOrWhiteSpace(DstFieldName))
		{
			return;
		}

		// if (string.IsNullOrWhiteSpace(Template))
		// {
		// 	return;
		// }

		var cols = source.Columns.ToList();
		cols.AddRange([new(PpString, DstFieldName)]);
		Table.Columns = cols;
		await Table.ClearAsync(ct);

		// // New table creating syntax
		// {
		// 	await Table
		// 		//
		// 		.ClearColumns()
		// 		.WithColumns(source)
		// 		.WithColumns((PpString, DstFieldName))
		// 		.RecreateAsync(ct);
		//
		// 	// Table.WithColumns(source);
		// 	// Table.WithColumns((PpString, DstFieldName));
		// 	// await Table.ClearAsync();
		// }

		// var engine = new RazorEngine();
		// var tpl = await engine.CompileAsync<CustomRazorTemplate>(Template, cancellationToken: ct);

		await using var appender = await Table.CreateAppenderAsync(ct);

		await foreach (var rec in source.QueryAllAsync(ct))
		{
			if (!rec.Fields.TryGetValue(SrcFieldName, out var src))
			{
				appender.Add(CreateRecord(rec, $"No field named '{SrcFieldName}'"));
				break;
			}

			var path = src.ValueAsString;

			try
			{
				// var doc = XElement.Parse(src.ValueAsString);
				// var q = (IEnumerable)doc.XPathEvaluate(Query);
				// var b = new StringBuilder();
				// if (q is string)
				// {

				// var eo = new ExpandoObject();
				// var eoColl = (ICollection<KeyValuePair<string, object>>)eo;
				// foreach (var kv in rec.Fields)
				// {
				// 	eoColl.Add(new KeyValuePair<string, object>(kv.Key, kv.Value.ValueAsString ?? string.Empty));
				// }

				// var res = tpl.Run(inst => inst.Rec = eo);

				// if(string.IsNullOrWhiteSpace())
				// {
				// 	throw new
				// }

				var fileCont = await File.ReadAllTextAsync(path);

				var newRec = new PpRecord()
				{
					//
					Fields = new Dictionary<string, PpField>(rec.Fields, StringComparer.OrdinalIgnoreCase),
				};
				newRec.Fields.Add(DstFieldName, fileCont);
				appender.Add(newRec);

				// New table creating syntax
				{
					var res = PpRecord.From(rec).With((DstFieldName, fileCont));

					appender.Add(res);
				}
			}
			catch (Exception ex)
			{
				_log.LogError(ex, "Error reading file: {Message}", ex.Message);
				Logs.Error($"Error reading file at path '{src.ValueAsString}': {ex.Message}");
			}
		}

		await Table.DoneAsync(ct);
	}

	private PpRecord CreateRecord(PpRecord file, string val) =>
		new() { Fields = new Dictionary<string, PpField>(file.Fields, StringComparer.OrdinalIgnoreCase) { { DstFieldName, val } } };
}
