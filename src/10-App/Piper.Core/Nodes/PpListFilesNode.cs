using Microsoft.Extensions.FileSystemGlobbing;
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
		_files = new(PpTable.GetTableName(this, nameof(OutFiles)));

		OutFiles = new(this, nameof(OutFiles)) { Table = () => _files };
	}

	public override string Color => "#8a2828";

	public override string Icon => "fa-regular fa-folder";

	public override string NodeType => "List Files";

	public override bool SupportsProgress => false;

	[PpParam("Path")]
	public string? InPath { get; set; }

	[PpParam("Pattern")]
	public string InPattern { get; set; } = "*";

	[PpParam("Max Files")]
	public int MaxFiles { get; set; } = 10_000;

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
			new("file__createdutc", PpDateTime),
			new("file__dir", PpString),
			new("file__ext", PpString),
			new("file__name", PpString),
			new("file__name_without_ext", PpString),
			new("file__path", PpString),
			new("file__size", PpInt32),
		];

		await _files.ClearAsync();

		var matcher = new Matcher();
		matcher.AddIncludePatterns([InPattern]);
		var it = matcher.GetResultsInFullPath(InPath);

		// var it = Directory.EnumerateFiles(
		// 	path: InPath,
		// 	searchPattern: InPattern,
		// 	enumerationOptions: new EnumerationOptions() { RecurseSubdirectories = true }
		// );

		var i = 0;

		{
			await using var appender = await _files.CreateAppenderAsync();

			foreach (var path in it)
			{
				if (++i > MaxFiles)
				{
					Logs.Warning($"Hit max file limit {MaxFiles}, while more files are found");
					break;
				}

				var fi = new FileInfo(path);

				appender.Add(
					new PpRecord()
					{
						Fields =
						{
							{ "rec__uuid", new(PpGuid, Guid.CreateVersion7()) },
							{ "file__createdutc", new(PpDateTime, fi.CreationTimeUtc) },
							{ "file__dir", new(PpString, Path.GetDirectoryName(path)) },
							{ "file__ext", new(PpString, Path.GetExtension(path)) },
							{ "file__name", new(PpString, Path.GetFileName(path)) },
							{ "file__name_without_ext", new(PpString, Path.GetFileNameWithoutExtension(path)) },
							{ "file__path", new(PpString, Path.GetFullPath(path)) },
							{ "file__size", new(PpInt32, (int)fi.Length) },
						},
					}
				);
			}
		}

		await _files.DoneAsync();
	}
}
