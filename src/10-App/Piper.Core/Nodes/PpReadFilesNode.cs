using Piper.Core.Db;

namespace Piper.Core.Nodes;

public class PpReadFilesNode : PpNodeBase
{
	public override string NodeType => "Read Files";

	// private PpDataFrame _outLines = new();
	private PpTable? _outLines;

	public PpReadFilesNode()
	{
		OutLines = new()
		{
			NodePortName = "Lines",
			Table = () => _outLines,
		};
	}

	public override string? Name { get; set; }

	[PpInput("File Paths")]
	public PpNodeInput InFiles { get; set; } = new() { NodePortName = "File Paths" };

	[PpParam("In Attribute")]
	public string? InAttr { get; set; }

	[PpParam("Max File Size")]
	public int MaxFileSize { get; set; } = 2_000_000; // 2MB

	[PpOutput("Lines")]
	public PpNodeOutput OutLines { get; }

	protected override async Task OnExecuteAsync()
	{
		var inTable = InFiles.Table();
		var inp = await DuckDbPpDb.Instance.Query2Async($"select * from {inTable.TableName}");

		var cols = inTable.Columns.ToList();
		cols.AddRange([
			new() { Name = "idx", },
			new() { Name = "line", },
		]);

		_outLines = new()
		{
			TableName = "readlines",
			Columns = cols,
		};

		await _outLines.ClearAsync();
		var i = 0;
		foreach (var file in inp)
		{
			// Get attribute
			// var field = file.Fields.FirstOrDefault(f =>
			// 	InFiles.AttributeName == null || f.Key == InFiles.AttributeName);
			var field = file.Fields.FirstOrDefault(f => f.Key?.Equals(InAttr, StringComparison.OrdinalIgnoreCase) ?? false);

			if (field.Value == null)
			{
				Console.WriteLine("No attributes");
				// await _outLines.AddAsync(file);
				continue;
			}

			// Read file
			var path = field.Value.ValueAsString;
			if (!File.Exists(path))
			{
				Console.WriteLine($"File at path '{path}' does not exist");
				// await _outLines.AddAsync(file);
				continue;
			}

			Console.WriteLine($"({i++}/{inp.Count}) Reading file at path {path}");

			var fileInfo = new FileInfo(path);
			if (fileInfo.Length > MaxFileSize)
			{
				Console.WriteLine($"File at path '{path}' exceeds max size, skipping. Increase '{MaxFileSize}' to include larger files.");
				continue;
			}

			var lines = await File.ReadAllLinesAsync(path);

			await _outLines.AddAsync(lines.Select((line, idx) =>
			{
				var ff = new Dictionary<string, PpField>(file.Fields,
					StringComparer.OrdinalIgnoreCase);
				ff["idx"] = new(idx);
				ff["line"] = new(line);

				return new PpRecord() { Fields = ff, };
			}));
		}
	}
}