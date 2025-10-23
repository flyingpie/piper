using Piper.Core.Attributes;
using Piper.Core.Data;
using static Piper.Core.Data.PpDataType;
using static Piper.Core.Data.PpPortDirection;

namespace Piper.Core.Nodes;

public class PpListFilesNode : PpNode
{
	private readonly PpTable _files = new("listfiles");

	public PpListFilesNode()
	{
		OutFiles = new(this, nameof(OutFiles))
		{
			Table = () => _files,
		};
	}

	public override string NodeType => "List Files";

	[PpParam("Path")]
	public string? InPath { get; set; }

	[PpParam("Pattern")]
	public string? InPattern { get; set; }

	[PpPort(Out, "Files")]
	public PpNodeOutput OutFiles { get; }

	protected override async Task OnExecuteAsync()
	{
		if (string.IsNullOrWhiteSpace(InPath))
		{
			Logs.Warning($"Missing value for param '{nameof(InPath)}'.");
			return;
		}

		if (string.IsNullOrWhiteSpace(InPattern))
		{
			Logs.Warning($"Missing value for param '{nameof(InPattern)}'.");
			return;
		}

		_files.Columns =
		[
			new("rec__uuid", PpString),
			new("file__path", PpString),
			// new("file.dir", PpString),
			// new("file.name", PpString),
			// new("file.name_without_ext", PpString),
			// new("file.ext", PpString),
		];

		await _files.ClearAsync();

		var dirs = Directory.GetFiles(
			path: InPath,
			searchPattern: InPattern,
			new EnumerationOptions()
			{
				RecurseSubdirectories = true, // TODO: Use glob instead
			});

		var records = dirs
			.Select(d => new PpRecord()
			{
				Fields =
				{
					{ "rec__uuid", new(Guid.CreateVersion7()) },
					{ "file__path", new(d) },
					// { "dir", new(Path.GetDirectoryName((string?)d)) },
					// { "file", new(Path.GetFileName((string?)d)) },
					// { "ext", new(Path.GetExtension((string?)d)) },
				},
			})
			.ToList();

		await _files.AddAsync(records);
		await _files.DoneAsync();
	}
}