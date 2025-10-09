using Blazor.Diagrams.Core.Models;
using Piper.Core.Attributes;
using Piper.Core.Data;
using static Piper.Core.Data.PpDataType;
using static Piper.Core.Data.PpPortDirection;

namespace Piper.Core.Nodes;

public class PpListFilesNode : PpNodeBase
{
	private readonly PpTable _files = new("listfiles");

	public PpListFilesNode()
	{
		OutFiles = new() { Table = () => _files, };
	}

	public override string NodeType => "List Files";

	[PpParam("Path")]
	public string? InPath { get; set; }

	[PpParam("Pattern")]
	public string? InPattern { get; set; }

	[PpPort(Out, "Files")]
	public PpNodeOutput OutFiles { get; }

	public override NodeModel ToNodeModel() => new GenericNodeModel<PpListFilesNode>();

	protected override async Task OnExecuteAsync()
	{
		if (string.IsNullOrWhiteSpace(InPath))
		{
			LogWarning($"Missing value for param '{nameof(InPath)}'.");
			return;
		}

		if (string.IsNullOrWhiteSpace(InPattern))
		{
			LogWarning($"Missing value for param '{nameof(InPattern)}'.");
			return;
		}

		_files.Columns =
		[
			new("uuid", PpString),
			new("path", PpString),
			new("dir", PpString),
			new("file", PpString),
			new("ext", PpString),
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
					{ "uuid", new(Guid.CreateVersion7()) },
					{ "path", new(d) },
					{ "dir", new(Path.GetDirectoryName((string?)d)) },
					{ "file", new(Path.GetFileName((string?)d)) },
					{ "ext", new(Path.GetExtension((string?)d)) },
				},
			})
			.ToList();

		await _files.AddAsync(records);
	}
}