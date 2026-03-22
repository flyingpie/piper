using System.Dynamic;
using Piper.Core.Attributes;
using Piper.Core.Data;
using Piper.Core.Utils;
using RazorEngineCore;
using static Piper.Core.Data.PpDataType;
using static Piper.Core.Data.PpPortDirection;

namespace Piper.Core.Nodes;

public class PpRazorNode : PpNode
{
	public PpRazorNode()
	{
		InRecords = new(this, nameof(InRecords));
		OutRecords = new(this, nameof(OutRecords));
	}

	public override string Color => "#8a2828";

	public override string Icon => "fa-regular fa-file-lines";

	public override string NodeType => "Razor Template";

	public override bool SupportsProgress => true;

	[PpParam("Template", Hint = PpParamHint.Code)]
	public string Template { get; set; } = "";

	[PpPort(In, "Records")]
	public PpNodeInput InRecords { get; }

	[PpPort(Out, "Records")]
	public PpNodeOutput OutRecords { get; }

	protected override async Task OnExecuteAsync()
	{
		if (!InRecords.IsConnected)
		{
			Logs.Warning($"Port '{InRecords}' not connected");
			return;
		}

		if (string.IsNullOrWhiteSpace(Template))
		{
			Logs.Warning($"Param '{Template}' not set");
			return;
		}

		var engine = new RazorEngine();
		var tpl = await engine.CompileAsync<CustomRazorTemplate>(Template);

		var inTable = InRecords.Output.Table;

		var cols = inTable.Columns.ToList();
		cols.AddRange([new(PpString, "razor")]);
		OutRecords.BaseTable.Columns = cols;
		await OutRecords.BaseTable.ClearAsync();

		{
			await using var appender = await OutRecords.BaseTable.CreateAppenderAsync();

			var i = 0;
			await foreach (var recIn in inTable.QueryAllAsync())
			{
				Progress = ((float)i) / inTable.Count;

				if (i % 1000 == 0)
				{
					Changed();
				}

				Logs.Info($"({i++}/{9999}) Executing Razor template");

				var eo = new ExpandoObject();
				var eoColl = (ICollection<KeyValuePair<string, object>>)eo;
				foreach (var kv in recIn.Fields)
				{
					eoColl.Add(new KeyValuePair<string, object>(kv.Key, kv.Value.ValueAsString ?? string.Empty));
				}
				// dynamic model = eo;

				// var json = PpJson.SerializeToString(recIn.Fields);
				// var model = PpJson.Deserialize<dynamic>(json);

				var res = tpl.Run(inst =>
				{
					inst.Rec = eo;
				});

				var recOut = new PpRecord()
				{
					Fields = new Dictionary<string, PpField>(recIn.Fields, StringComparer.OrdinalIgnoreCase)
					{
						// { "idx", $"{idx}" },
						{ "razor", res },
					},
				};

				appender.Add(recOut);
			}
		}

		await OutRecords.BaseTable.DoneAsync();
	}

	// public PpRecord CreateRecord(PpRecord file, int idx, string line) =>
	// 	new()
	// 	{
	// 		Fields = new Dictionary<string, PpField>(file.Fields, StringComparer.OrdinalIgnoreCase)
	// 		{
	// 			// { "idx", $"{idx}" },
	// 			{ "razor", line },
	// 		},
	// 	};

	public class CustomRazorTemplate : RazorEngineTemplateBase
	{
		public dynamic Rec { get; set; }
	}
}