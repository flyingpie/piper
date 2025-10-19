using Piper.Core.Attributes;
using Piper.Core.Data;
using static Piper.Core.Data.PpDataType;
using static Piper.Core.Data.PpPortDirection;

namespace Piper.Core.Nodes;

public class PpReadFilesNode : PpNode
{
	private readonly PpTable _outLines = new("readlines");

	public PpReadFilesNode()
	{
		InFiles = new(this, nameof(InFiles));
		OutLines = new(this, nameof(OutLines)) { Table = () => _outLines, };
	}

	public override string NodeType => "Read Files";

	[PpPort(In, "File Paths")]
	public PpNodeInput InFiles { get; }

	[PpParam("In Attribute")]
	public string? InAttr { get; set; }

	[PpParam("Max File Size")]
	public int MaxFileSize { get; set; } = 2_000_000; // 2MB

	[PpPort(Out, "Lines")]
	public PpNodeOutput OutLines { get; }

	protected override async Task OnExecuteAsync()
	{
		if (!InFiles.IsConnected)
		{
			Logs.Warning($"Port '{InFiles}' not connected");
			return;
		}

		if (string.IsNullOrWhiteSpace(InAttr))
		{
			Logs.Warning($"Param '{InAttr}' not set");
			return;
		}

		var inTable = InFiles.Output.Table();

		var cols = inTable.Columns.ToList();
		cols.AddRange(
		[
			new("idx", PpString),
			new("line", PpString),
		]);

		_outLines.Columns = cols;

		await _outLines.ClearAsync();
		var i = 0;
		await foreach (var file in inTable.QueryAllAsync())
		{
			// Get attribute
			var field = file.Fields.FirstOrDefault(f => f.Key?.Equals(InAttr, StringComparison.OrdinalIgnoreCase) ?? false);

			if (string.IsNullOrWhiteSpace(field.Value?.ValueAsString))
			{
				Logs.Warning($"Record does not have an attribute with name '{InAttr}'");
				await _outLines.AddAsync(CreateRecord(file, -1, string.Empty));
				continue;
			}

			// Read file
			var path = field.Value.ValueAsString;
			if (!File.Exists(path))
			{
				Logs.Warning($"File at path '{path}' does not exist");
				await _outLines.AddAsync(CreateRecord(file, -1, string.Empty));
				continue;
			}

			Logs.Info($"({i++}/{9999}) Reading file at path {path}");

			var fileInfo = new FileInfo(path);
			if (fileInfo.Length > MaxFileSize)
			{
				Logs.Warning($"File at path '{path}' exceeds max size, skipping. Increase '{MaxFileSize}' to include larger files.");
				continue;
			}

			var lines = await File.ReadAllLinesAsync(path);

			await _outLines.AddAsync(lines.Select((line, idx) => CreateRecord(file, idx, line)));
		}

		await _outLines.DoneAsync();
	}

	public PpRecord CreateRecord(PpRecord file, int idx, string line) =>
		new()
		{
			Fields = new Dictionary<string, PpField>(file.Fields, StringComparer.OrdinalIgnoreCase)
			{
				{ "idx", $"{idx}" },
				{ "line", line },
			},
		};
}