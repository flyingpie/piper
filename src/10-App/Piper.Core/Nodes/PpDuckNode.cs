using Piper.Core.Attributes;
using Piper.Core.Data;
using Piper.Core.Db;
using static Piper.Core.Data.PpPortDirection;

namespace Piper.Core.Nodes;

public class PpDuckNode : PpNode
{
	public PpDuckNode()
	{
		InRecords1 = new(this, nameof(InRecords1));
		InRecords2 = new(this, nameof(InRecords2));
		InRecords3 = new(this, nameof(InRecords3));
		InRecords4 = new(this, nameof(InRecords4));
		InRecords5 = new(this, nameof(InRecords5));

		OutRecords = new(this, nameof(OutRecords), new(PpTable.GetTableName(this, nameof(OutRecords))));
	}

	public override string NodeType => "SQL Query";

	public override string Icon => "fa-solid fa-database";

	public override string Color => "#326590";

	public override bool SupportsProgress => false;

	[PpPort(In, "Records 1")]
	public PpNodeInput InRecords1 { get; set; }

	[PpPort(In, "Records 2")]
	public PpNodeInput InRecords2 { get; set; }

	[PpPort(In, "Records 3")]
	public PpNodeInput InRecords3 { get; set; }

	[PpPort(In, "Records 4")]
	public PpNodeInput InRecords4 { get; set; }

	[PpPort(In, "Records 5")]
	public PpNodeInput InRecords5 { get; set; }

	[PpParam("Query", Hint = PpParamHint.Code)]
	public string Query { get; set; } = "";

	[PpPort(Out, "Records")]
	public PpNodeOutput OutRecords { get; }

	protected override async Task OnExecuteAsync()
	{
		if (!InRecords1.IsConnected)
		{
			Logs.Warning($"Port '{InRecords1}' not connected");
			return;
		}

		// var inTable = InRecords1.Output.Table;
		var inTables = new List<PpTable>();
		if (InRecords1.IsConnected)
		{
			inTables.Add(InRecords1.Output.Table);
		}
		if (InRecords2.IsConnected)
		{
			inTables.Add(InRecords2.Output.Table);
		}
		if (InRecords3.IsConnected)
		{
			inTables.Add(InRecords3.Output.Table);
		}
		if (InRecords4.IsConnected)
		{
			inTables.Add(InRecords4.Output.Table);
		}
		if (InRecords5.IsConnected)
		{
			inTables.Add(InRecords5.Output.Table);
		}

		PpDbAppender? appender = null;

		try
		{
			// await foreach (var rec in inTable.QueryAsync(Query))
			await foreach (var rec in PpDb.Instance.QueryAsync(inTables, Query))
			{
				if (appender == null)
				{
					OutRecords.Table.Columns = rec.Fields.Select(kv => new PpColumn(kv.Key, kv.Value.DataType)).ToList();
					await OutRecords.Table.ClearAsync();
					appender = await OutRecords.Table.CreateAppenderAsync();
				}

				appender.Add(rec);
			}
		}
		finally
		{
			await (appender?.DisposeAsync() ?? ValueTask.CompletedTask);
		}

		await OutRecords.Table.DoneAsync();
	}
}
