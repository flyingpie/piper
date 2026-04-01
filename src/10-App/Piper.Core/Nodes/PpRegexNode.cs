using Piper.Core.Attributes;
using Piper.Core.Data;
using static Piper.Core.Data.PpDataType;
using static Piper.Core.Data.PpPortDirection;

namespace Piper.Core.Nodes;

public class PpRegexNode : PpNode
{
	public PpRegexNode()
	{
		InRecords = new(this, nameof(InRecords));

		OutMatch = new(this, nameof(OutMatch));
		OutNoMatch = new(this, nameof(OutNoMatch));
	}

	public override string Color => "#8a2828";

	public override string Icon => "fa-solid fa-r";

	public override string NodeType => "Regex";

	public override bool SupportsProgress => true;

	[PpParam("Pattern")]
	public string? InPattern { get; set; }

	[PpParam("In Attribute")]
	public string? InAttribute { get; set; }

	[PpParam("Out Attribute")]
	public string OutAttribute { get; set; } = "capture";

	[PpPort(In, "Input")]
	public PpNodeInput InRecords { get; set; }

	[PpPort(Out, "Match")]
	public PpNodeOutput OutMatch { get; }

	[PpPort(Out, "No Match")]
	public PpNodeOutput OutNoMatch { get; }

	protected override async Task OnExecuteAsync()
	{
		if (!InRecords.IsConnected)
		{
			Logs.Warning($"Port '{InRecords}' not connected, stopping");
			return;
		}

		if (string.IsNullOrWhiteSpace(InAttribute))
		{
			Logs.Warning("No attribute specified, stopping");
			return;
		}

		if (string.IsNullOrWhiteSpace(OutAttribute))
		{
			Logs.Warning("No attribute specified, stopping");
			return;
		}

		// Read in
		var inTable = InRecords.Table;

		var cols1 = inTable.Columns.ToList();
		cols1.AddRange([new(PpString, OutAttribute)]);
		OutMatch.BaseTable.Columns = cols1;
		await OutMatch.BaseTable.ClearAsync();

		var regex = new Regex(InPattern ?? string.Empty, RegexOptions.Compiled);

		{
			await using var appender = await OutMatch.BaseTable.CreateAppenderAsync();
			// await using var appender2 = await OutNoMatch.BaseTable.CreateAppenderAsync();

			await foreach (var rec in inTable.QueryAllAsync())
			{
				// Get attribute
				var field = rec.Fields.FirstOrDefault(f => f.Key?.Equals(InAttribute, StringComparison.OrdinalIgnoreCase) ?? false);

				// TODO: Put in JSON struct to support multiple capture groups?
				var matches = regex.Matches(field.Value.ValueAsString ?? string.Empty);

				foreach (Match match in matches)
				{
					rec.Fields[OutAttribute] = new(PpString, match.Value);

					appender.Add(rec);
				}
			}
		}

		await OutMatch.BaseTable.DoneAsync();
		// await OutNoMatch.BaseTable.DoneAsync();
	}
}
