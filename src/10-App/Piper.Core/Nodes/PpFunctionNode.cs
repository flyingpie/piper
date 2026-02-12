using Piper.Core.Attributes;
using Piper.Core.Data;
using Piper.Core.Functions;

namespace Piper.Core.Nodes;

public class PpFunctionNode : PpNode
{
	public PpFunctionNode()
	{
		InRecords = new(this, nameof(InRecords));
		OutRecords = new(this, nameof(OutRecords), new(PpTable.GetTableName(this, nameof(OutRecords))));
	}

	public override string Color => "#8a2828";

	public override string Icon => "fa-solid fa-code";

	public override string NodeType => "Function";

	public override bool SupportsProgress => false;

	[PpParam("Function")]
	public string Function { get; set; }

	[PpParam("InAttr")]
	public string InAttr { get; set; }

	[PpParam("OutAttr")]
	public string OutAttr { get; set; }

	[PpPort(PpPortDirection.Out, "InRecords")]
	public PpNodeInput InRecords { get; set; }

	[PpPort(PpPortDirection.Out, "OutRecords")]
	public PpNodeOutput OutRecords { get; }

	protected override async Task OnExecuteAsync()
	{
		var cols = InRecords.Output.Table.Columns.ToList();
		cols.Add(new("out1", PpDataType.PpString));
		OutRecords.Table.Columns.Clear();
		OutRecords.Table.Columns.AddRange(cols);
		await OutRecords.Table.ClearAsync();

		var func = new PpReverseFunc();

		{
			await using var appender = await OutRecords.Table.CreateAppenderAsync();

			await foreach (var rec in InRecords.Output.Table.QueryAllAsync())
			{
				var val = rec.Fields[InAttr].ValueAsString;
				var res = await func.ExecuteAsync(val);
				rec.Fields[OutAttr] = new PpField(PpDataType.PpString, res);

				appender.Add(new PpRecord() { Fields = new Dictionary<string, PpField>(rec.Fields, StringComparer.OrdinalIgnoreCase) });
			}
		}

		await OutRecords.Table.DoneAsync();
	}
}
