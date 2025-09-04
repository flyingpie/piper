using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Piper.Core.UnitTest;

public class PpListFilesNode : IPpNode
{
	private PpDataFrame _files = new();

	public string? Name { get; set; }

	public PpNodeInput InPath { get; set; } = new();

	public PpNodeInput InPattern { get; set; } = new();

	public PpNodeOutput OutFiles { get; } = new();

	public async Task ExecuteAsync()
	{
		_files.Records.Clear();

		var dirs = Directory.GetFiles(path: InPath.Value, searchPattern: InPattern.Value,
			new EnumerationOptions()
			{
				RecurseSubdirectories = true, // TODO: Use glob instead
			});

		_files = new()
		{
			Records = dirs
				.Select(d => new PpRecord()
				{
					Fields =
					{
						{ "uuid", new(Guid.CreateVersion7()) },
						{ "path", new(d) },
						{ "dir", new(Path.GetDirectoryName((string?)d)) },
						{ "file", new(Path.GetFileName((string?)d)) },
						{ "ext", new(Path.GetExtension((string?)d)) },
					}
				})
				.ToList(),
		};

		OutFiles.DataFrame = () => _files;
	}
}