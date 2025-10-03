namespace Piper.Core.Nodes;

public class PpReadFilesNode : IPpNode
{
	public string NodeType => "Read Files";

	private PpDataFrame _outLines = new();

	public string? Name { get; set; }

	public bool IsExecuting { get; }

	[PpInput("File Paths")]
	public PpNodeInput InFiles { get; set; } = new() { NodePortName = "File Paths" };

	[PpParam("In Attribute")]
	public string? InAttr { get; set; }

	[PpOutput("Lines")]
	public PpNodeOutput OutLines { get; } = new() { NodePortName = "Lines" };

	public async Task ExecuteAsync()
	{
		_outLines.Records.Clear();

		foreach (var file in InFiles.DataFrame().Records)
		{
			// Get attribute
			// var field = file.Fields.FirstOrDefault(f =>
			// 	InFiles.AttributeName == null || f.Key == InFiles.AttributeName);
			var field = file.Fields.FirstOrDefault(f => f.Key?.Equals(InAttr, StringComparison.OrdinalIgnoreCase) ?? false);

			if (field.Value == null)
			{
				Console.WriteLine("No attributes");
				_outLines.Records.Add(file);
				continue;
			}

			// Read file
			var path = field.Value.ValueAsString;
			if (!File.Exists(path))
			{
				Console.WriteLine($"File at path '{path}' does not exist");
				_outLines.Records.Add(file);
				continue;
			}

			var lines = await File.ReadAllLinesAsync(path);

			_outLines.Records.AddRange(lines.Select((line, idx) =>
			{
				var ff = new Dictionary<string, PpField>(file.Fields,
					StringComparer.OrdinalIgnoreCase);
				ff["idx"] = new(idx);
				ff["line"] = new(line);

				return new PpRecord() { Fields = ff, };
			}));
		}

		OutLines.DataFrame = () => _outLines;
	}
}