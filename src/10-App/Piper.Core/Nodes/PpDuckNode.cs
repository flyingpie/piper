using Piper.Core.Attributes;
using Piper.Core.Data;
using static Piper.Core.Data.PpPortDirection;

namespace Piper.Core.Nodes;

public class PpDuckNode : PpNode
{
	private readonly PpTable _outLines;

	public PpDuckNode()
	{
		_outLines = new(PpTable.GetTableName(this, nameof(OutRecords)));

		InRecords = new(this, nameof(InRecords));
		OutRecords = new(this, nameof(OutRecords)) { Table = () => _outLines };
	}

	public override string NodeType => "SQL Query";

	public override string Icon => "fa-solid fa-database";

	public override string Color => "#326590";

	public override bool SupportsProgress => false;

	[PpPort(In, "Records")]
	public PpNodeInput InRecords { get; set; }

	[PpParam("Query", Hint = PpParamHint.Code)]
	public string Query { get; set; } = "";

	[PpPort(Out, "Records")]
	public PpNodeOutput OutRecords { get; }

	protected override async Task OnExecuteAsync()
	{
		if (!InRecords.IsConnected)
		{
			Logs.Warning($"Port '{InRecords}' not connected");
			return;
		}

		var inTable = InRecords.Output.Table();

		PpDbAppender? appender = null;

		try
		{
			await foreach (var rec in inTable.QueryAsync(Query))
			{
				if (appender == null)
				{
					_outLines.Columns = rec.Fields.Select(kv => new PpColumn(kv.Key, kv.Value.DataType)).ToList();
					await _outLines.ClearAsync();
					appender = await _outLines.CreateAppenderAsync();
				}

				appender.Add(rec);
			}
		}
		finally
		{
			await (appender?.DisposeAsync() ?? ValueTask.CompletedTask);
		}

		await _outLines.DoneAsync();
	}
}
