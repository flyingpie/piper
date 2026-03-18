using Piper.Core.Attributes;
using Piper.Core.Data;
using Piper.Core.Db;
using static Piper.Core.Data.PpDataType;
using static Piper.Core.Data.PpPortDirection;

namespace Piper.Core.Nodes;

public class PpXsltNode : PpNode
{
	public PpXsltNode()
	{
		InRecords = new(this, nameof(InRecords));
		OutRecords = new(this, nameof(OutRecords));
	}

	public override string Color => "#8a2828";

	public override string Icon => "fa-regular fa-file-lines";

	public override string NodeType => "XSLT";

	public override bool SupportsProgress => true;

	[PpParam("Stylesheet", Hint = PpParamHint.Code)]
	public string? Stylesheet { get; set; }

	[PpPort(PpPortDirection.In, "Records")]
	public PpNodeInput InRecords { get; }

	[PpPort(PpPortDirection.Out, "Records")]
	public PpNodeOutput OutRecords { get; }

	protected override async Task OnExecuteAsync()
	{
		if (!InRecords.IsConnected)
		{
			Logs.Warning($"Port '{InRecords}' not connected");
			return;
		}

		var inTable = InRecords.Output.Table;

		var cols = inTable.Columns.ToList();
		cols.AddRange([new(PpString, "idx"), new(PpString, "line")]);
		OutRecords.BaseTable.Columns = cols;
		await OutRecords.BaseTable.ClearAsync();

		var appender = await OutRecords.BaseTable.CreateAppenderAsync();

		await foreach (var rec in InRecords.Table.QueryAllAsync())
		{
			appender.Add(rec);
		}

		await OutRecords.BaseTable.DoneAsync();
	}
}

// public class PpXPathNode : PpNode
// {
// 	public PpXPathNode()
// 	{
// 		InRecords = new(this, nameof(InRecords));
// 		OutRecords = new(this, nameof(OutRecords));
// 	}
//
// 	public override string Color => "#8a2828";
//
// 	public override string Icon => "fa-regular fa-file-lines";
//
// 	public override string NodeType => "XPath";
//
// 	public override bool SupportsProgress => true;
//
// 	[PpParam("Query", Hint = PpParamHint.Code)]
// 	public string? Query { get; set; }
//
// 	[PpPort(PpPortDirection.In, "Records")]
// 	public PpNodeInput InRecords { get; }
//
// 	[PpPort(PpPortDirection.Out, "Records")]
// 	public PpNodeOutput OutRecords { get; }
//
// 	protected override async Task OnExecuteAsync()
// 	{
// 		if (!InRecords.IsConnected)
// 		{
// 			Logs.Warning($"Port '{InRecords}' not connected");
// 			return;
// 		}
//
// 		var inTable = InRecords.Output.Table;
//
// 		var cols = inTable.Columns.ToList();
// 		cols.AddRange([new(PpString, "idx"), new(PpString, "line")]);
// 		OutRecords.BaseTable.Columns = cols;
// 		await OutRecords.BaseTable.ClearAsync();
//
// 		var appender = await OutRecords.BaseTable.CreateAppenderAsync();
//
// 		await foreach (var rec in InRecords.Table.QueryAllAsync())
// 		{
// 			// if (appender == null)
// 			// {
// 			// 	OutRecords.BaseTable.Columns = rec.Fields.Select(kv => new PpColumn(kv.Value.DataType, kv.Key)).ToList();
// 			// 	await OutRecords.BaseTable.ClearAsync();
// 			// 	appender = await OutRecords.BaseTable.CreateAppenderAsync();
// 			// }
//
// 			appender.Add(rec);
// 		}
//
// 		await OutRecords.BaseTable.DoneAsync();
// 	}
// }
