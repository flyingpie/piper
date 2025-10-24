using Piper.Core.Attributes;
using Piper.Core.Data;
using static Piper.Core.Data.PpDataType;
using static Piper.Core.Data.PpPortDirection;

namespace Piper.Core.Nodes;

public class PpDuckNode : PpNode
{
	private PpTable _outLines = new("todo");

	public PpDuckNode()
	{
		InRecords = new(this, nameof(InRecords));
		OutRecords = new(this, nameof(OutRecords)) { Table = () => _outLines };
	}

	[PpParam("Query")]
	public string Query { get; set; } = "";

	[PpPort(In, "Records")]
	public PpNodeInput InRecords { get; set; }

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

		var isCleared = false;

		// await foreach (var rec in inTable.QueryAsync(Query))
		await foreach (var rec in inTable.QueryAsync(Query))
		{
			if (!isCleared)
			{
				_outLines.Columns = rec.Fields.Select(kv => new PpColumn(kv.Key, kv.Value.DataType)).ToList();

				await _outLines.ClearAsync();

				isCleared = true;
			}

			await _outLines.AddAsync(rec);
		}

		await _outLines.DoneAsync();
	}
}