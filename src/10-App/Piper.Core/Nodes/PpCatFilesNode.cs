using Blazor.Diagrams.Core.Models;

namespace Piper.Core.Nodes;

public class PpCatFilesNode : NodeModel, IPpNode
{
	public PpCatFilesNode()
	{
		InFilesPort = AddPort(PortAlignment.Left);
		OutLinesPort = AddPort(PortAlignment.Right);
	}

	public string NodeType => "Read Files";

	private PpDataFrame _outLines = new();

	public string? Name { get; set; }

	public PpNodeInput InFiles { get; set; } = new();

	public PortModel InFilesPort { get; set; }

	public PpNodeOutput OutLines { get; } = new();

	public PortModel OutLinesPort { get; set; }

	public async Task ExecuteAsync()
	{
		_outLines.Records.Clear();

		foreach (var file in InFiles.DataFrame().Records)
		{
			// Get attribute
			var field = file.Fields.FirstOrDefault(f =>
				InFiles.AttributeName == null || f.Key == InFiles.AttributeName);

			if (field.Value == null)
			{
				Console.WriteLine("No attributes");
				continue;
			}

			// Read file
			var path = field.Value.ValueAsString;
			if (!File.Exists(path))
			{
				Console.WriteLine($"File at path '{path}' does not exist");
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