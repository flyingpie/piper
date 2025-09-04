using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Piper.Core.UnitTest;

public class PpCatFilesNode : IPpNode
{
	private PpDataFrame _outLines = new();

	public string? Name { get; set; }

	public PpNodeInput InFiles { get; set; } = new();

	public PpNodeOutput OutLines { get; } = new();

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

				return new PpRecord()
				{
					Fields = ff,
				};
			}));
		}

		OutLines.DataFrame = () => _outLines;
	}
}