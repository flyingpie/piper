using Microsoft.Extensions.FileSystemGlobbing;
using Piper.Core.Attributes;
using Piper.Core.Data;
using static Piper.Core.Data.PpDataType;
using static Piper.Core.Data.PpPortDirection;

namespace Piper.Core.Nodes;

public class PpListFilesNode : PpNode
{
	public PpListFilesNode()
	{
		OutFiles = new(this, nameof(OutFiles), new(PpTable.GetTableName(this, nameof(OutFiles))));
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

		OutFiles.Table.Columns =
		[
			new("rec__uuid", PpGuid),
			// new("file", PpDataType.PpJson),
			new("file", PpDataType.PpString),
			// new("file__createdutc", PpDateTime),
			// new("file__dir", PpString),
			// new("file__ext", PpString),
			// new("file__name", PpString),
			// new("file__name_without_ext", PpString),
			// new("file__path", PpString),
			// new("file__size", PpInt32),
		];

		await OutFiles.Table.ClearAsync();

		var matcher = new Matcher();
		matcher.AddIncludePatterns([InPattern]);
		var it = matcher.GetResultsInFullPath(InPath);

		var i = 0;

		{
			await using var appender = await OutFiles.Table.CreateAppenderAsync();

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
							{
								"file",
								new(
									PpString,
									// PpDataType.PpJson,
									Piper.Core.Utils.PpJson.SerializeToString(
										new
										{
											createdutc = fi.CreationTimeUtc,
											dir = Path.GetDirectoryName(path),
											ext = Path.GetExtension(path),
											name = Path.GetFileName(path),
											name_without_ext = Path.GetFileNameWithoutExtension(path),
											path = Path.GetFullPath(path),
											size = (int)fi.Length,
										}
									)
								)
							},
							// { "file__createdutc", new(PpDateTime, fi.CreationTimeUtc) },
							// { "file__dir", new(PpString, Path.GetDirectoryName(path)) },
							// { "file__ext", new(PpString, Path.GetExtension(path)) },
							// { "file__name", new(PpString, Path.GetFileName(path)) },
							// { "file__name_without_ext", new(PpString, Path.GetFileNameWithoutExtension(path)) },
							// { "file__path", new(PpString, Path.GetFullPath(path)) },
							// { "file__size", new(PpInt32, (int)fi.Length) },
						},
					}
				);
			}
		}

		await OutFiles.Table.DoneAsync();
	}
}
