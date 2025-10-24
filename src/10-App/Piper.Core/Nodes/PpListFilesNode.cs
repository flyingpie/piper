using MoreLinq;
using Piper.Core.Attributes;
using Piper.Core.Data;
using static Piper.Core.Data.PpDataType;
using static Piper.Core.Data.PpPortDirection;

namespace Piper.Core.Nodes;

public class PpListFilesNode : PpNode
{
	private readonly PpTable _files;

	public PpListFilesNode()
	{
		_files = new("listfiles");

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
			new("rec__uuid", PpGuid),
			new("file__name", PpString),
			new("file__ext", PpString),
			new("file__size", PpInt32),
			// new("file__createdutc", PpDateTime),

			// new("file.dir", PpString),
			// new("file.name", PpString),
			// new("file.name_without_ext", PpString),
			// new("file.ext", PpString),
		];

		// { "rec_uuid", new(PpGuid, Guid.CreateVersion7()) },
		// { "file__name", new(PpString, fi.Name) },
		// { "file__size", new(PpInt, fi.Length) },
		// { "file__createdutc", new(PpDateTime, fi.CreationTimeUtc) },

		await _files.ClearAsync();

		// var dirs = Directory.GetFiles(
		// 	path: InPath,
		// 	searchPattern: InPattern,
		// 	new EnumerationOptions()
		// 	{
		// 		RecurseSubdirectories = true, // TODO: Use glob instead
		// 	});
		//
		// var records = dirs
		// 	.Select(d => new PpRecord()
		// 	{
		// 		Fields =
		// 		{
		// 			{ "rec__uuid", new(Guid.CreateVersion7()) },
		// 			{ "file__path", new(d) },
		// 			// { "dir", new(Path.GetDirectoryName((string?)d)) },
		// 			// { "file", new(Path.GetFileName((string?)d)) },
		// 			// { "ext", new(Path.GetExtension((string?)d)) },
		// 		},
		// 	})
		// 	.ToList();

		// var it = Directory.EnumerateFiles(
		// 	path: InPath,
		// 	searchPattern: InPattern,
		// 	enumerationOptions: new EnumerationOptions()
		// 	{
		// 		RecurseSubdirectories = true,
		// 	});

		var files = Directory.GetFiles(
			path: InPath,
			searchPattern: InPattern,
			new EnumerationOptions()
			{
				RecurseSubdirectories = true, // TODO: Use glob instead
			});

		var i = 0;

		// foreach (var path in it)
		foreach (var path in files)
		{
			if (++i > 100)
			{
				break;
			}

			// var fi = new FileInfo(path);

			await _files.AddAsync([new()
			{
				Fields =
				{
					{ "rec__uuid", new(PpGuid, Guid.CreateVersion7()) },
					{ "file__name", new(PpString, Path.GetFileName(path)) },
					{ "file__ext", new(PpString, Path.GetExtension(path)) },
					// { "file__size", new(PpInt32, fi.Length) },
					{ "file__size", new(PpInt32, 0) },
					// { "file__createdutc", new(PpDateTime, fi.CreationTimeUtc) },
				},
			}]);
		}

		await _files.DoneAsync();
	}
}