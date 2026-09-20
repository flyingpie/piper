using Piper.Core.Attributes;
using Piper.Core.Data;
using Piper.Core.Functions;

namespace Piper.Core.Nodes;

public class PpFunctionNode : PpNode
{
	public PpFunctionNode()
	{
		InRecords = new(this, nameof(InRecords));
		OutRecords = new(this, nameof(OutRecords));
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
		// TODO
		// var cols = InRecords.Output.Table._columns.ToList();
		// cols.Add(new(PpDataType.PpString, "out1"));
		// OutRecords.BaseTable._columns.Clear();
		// OutRecords.BaseTable._columns.AddRange(cols);
		// OutRecords.BaseTable.Clear();
		//
		// var func = new PpReverseFunc();
		//
		// {
		// 	await using var appender = await OutRecords.BaseTable.CreateAppenderAsync();
		//
		// 	await foreach (var rec in InRecords.Output.Table.QueryAllAsync())
		// 	{
		// 		var val = rec.Fields[InAttr].ValueAsString;
		// 		var res = await func.ExecuteAsync(val);
		// 		rec.Fields[OutAttr] = new PpField(PpDataType.PpString, res);
		//
		// 		appender.AddAsync(new PpRecord() { Fields = new Dictionary<string, PpField>(rec.Fields, StringComparer.OrdinalIgnoreCase) });
		// 	}
		// }
		//
		// await OutRecords.BaseTable.DoneAsync();
	}
}
