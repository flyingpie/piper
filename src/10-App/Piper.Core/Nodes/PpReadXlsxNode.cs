using Piper.Core.Attributes;
using Piper.Core.Data;
using Piper.Core.Db;
using static Piper.Core.Data.PpPortDirection;

namespace Piper.Core.Nodes;

public class PpReadXlsxNode : PpNode
{
	private readonly PpTable _outLines;

	public PpReadXlsxNode()
	{
		_outLines = new(PpTable.GetTableName(this, nameof(OutRows)));
		OutRows = new(this, nameof(OutRows)) { Table = () => _outLines, };
	}

	public override string Color => "#8a2828";

	public override string Icon => "fa-regular fa-file-lines";

	public override string NodeType => "Read XSLX";

	public override bool SupportsProgress => true;

	[PpParam("Path")]
	public string Path { get; set; } = "";

	[PpParam("Sheet")]
	public string Sheet { get; set; } = "";

	[PpParam("Header")]
	public bool HasHeaderRow { get; set; } = true;

	[PpParam("AllVarchar")]
	public bool AllVarchar { get; set; }

	[PpPort(Out, "Rows")]
	public PpNodeOutput OutRows { get; }

	protected override async Task OnExecuteAsync()
	{
		if (string.IsNullOrWhiteSpace(Path))
		{
			Logs.Warning($"Param '{nameof(Path)}' not set");
			return;
		}

		if (!File.Exists(Path))
		{
			Logs.Warning($"Param '{nameof(Path)}' points to file '{Path}', which does not exist");
			return;
		}

		await PpDb.Instance.V_NonQueryRawAsync($"""
			create or replace table "{_outLines.TableName}"
				as select * from read_xlsx('{Path}', header = {HasHeaderRow.ToString().ToUpperInvariant()}, all_varchar = {AllVarchar.ToString().ToUpperInvariant()})
			""");
		await _outLines.InitAsync();
	}
}