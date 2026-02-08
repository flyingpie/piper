using Piper.Core.Attributes;
using Piper.Core.Data;
using static Piper.Core.Data.PpDataType;
using static Piper.Core.Data.PpPortDirection;

namespace Piper.Core.Nodes;

public class PpReadFilesNode : PpNode
{
	private readonly PpTable _outLines;

	public PpReadFilesNode()
	{
		_outLines = new(PpTable.GetTableName(this, nameof(OutLines)));

		InFiles = new(this, nameof(InFiles));
		OutLines = new(this, nameof(OutLines)) { Table = () => _outLines };
	}

	public override string Color => "#8a2828";

	public override string Icon => "fa-regular fa-file-lines";

	public override string NodeType => "Read Files";

	public override bool SupportsProgress => true;

	[PpPort(In, "File Paths")]
	public PpNodeInput InFiles { get; }

	[PpParam("In Attribute")]
	public string? InAttr { get; set; }

	[PpParam("Max File Size")]
	public int MaxFileSize { get; set; } = 2_000_000; // 2MB

	[PpParam("Split Lines")]
	public bool SplitLines { get; set; }

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
		cols.AddRange([new("idx", PpString), new("line", PpString)]);
		_outLines.Columns = cols;
		await _outLines.ClearAsync();

		{
			await using var appender = await _outLines.CreateAppenderAsync();

			var i = 0;
			await foreach (var file in inTable.QueryAllAsync())
			{
				Progress = ((float)i) / inTable.Count;

				if (i % 1000 == 0)
				{
					Changed();
				}

				// Get attribute
				var field = file.Fields.FirstOrDefault(f => f.Key?.Equals(InAttr, StringComparison.OrdinalIgnoreCase) ?? false);

				if (string.IsNullOrWhiteSpace(field.Value?.ValueAsString))
				{
					Logs.Warning($"Record does not have an attribute with name '{InAttr}'");
					appender.Add(CreateRecord(file, -1, string.Empty));
					continue;
				}

				// Read file
				var path = field.Value.ValueAsString;
				if (!File.Exists(path))
				{
					Logs.Warning($"File at path '{path}' does not exist");
					appender.Add(CreateRecord(file, -1, string.Empty));
					continue;
				}

				Logs.Info($"({i++}/{9999}) Reading file at path {path}");

				var fileInfo = new FileInfo(path);
				if (fileInfo.Length > MaxFileSize)
				{
					Logs.Warning($"File at path '{path}' exceeds max size, skipping. Increase '{MaxFileSize}' to include larger files.");
					continue;
				}

				if (SplitLines)
				{
					var lines = await File.ReadAllLinesAsync(path);

					for (var j = 0; j < lines.Length; j++)
					{
						var line = lines[j];
						appender.Add(CreateRecord(file, j, line));
					}
				}
				else
				{
					var text = await File.ReadAllTextAsync(path);
					appender.Add(CreateRecord(file, 0, text));
				}
			}
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

	// public PpRecord CreateRecord(PpRecord file, int idx, string[] lines) =>
	// 	new()
	// 	{
	// 		Fields = new Dictionary<string, PpField>(file.Fields, StringComparer.OrdinalIgnoreCase)
	// 		{
	// 			{ "idx", $"{idx}" },
	// 			{ "line", lines },
	// 		},
	// 	};
}
