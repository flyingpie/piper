using Microsoft.Extensions.FileSystemGlobbing;
using Piper.Core.Attributes;
using Piper.Core.Data;
using Piper.Core.Utils;
using static Piper.Core.Data.PpDataType;
using static Piper.Core.Data.PpPortDirection;

namespace Piper.Core.Nodes;

public class PpListFilesNode : PpNode
{
	public PpListFilesNode()
	{
		OutFiles = new(this, nameof(OutFiles));
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

		// OutFiles.BaseTable.Columns =
		// [
		// 	new(PpGuid, "rec__uuid"),
		// 	// new("file", PpDataType.PpJson),
		// 	new(PpDataType.PpString, "file"),
		// 	// new("file__createdutc", PpDateTime),
		// 	// new("file__dir", PpString),
		// 	// new("file__ext", PpString),
		// 	// new("file__name", PpString),
		// 	// new("file__name_without_ext", PpString),
		// 	// new("file__path", PpString),
		// 	// new("file__size", PpInt32),
		// ];

		// await OutFiles.BaseTable.ClearAsync();
		OutFiles.BaseTable.Clear();

		var matcher = new Matcher();
		matcher.AddIncludePatterns([InPattern]);
		var it = matcher.GetResultsInFullPath(InPath);

		var i = 0;

		{
			await using var appender = await OutFiles.BaseTable.CreateAppenderAsync();

			foreach (var path in it)
			{
				if (++i > MaxFiles)
				{
					Logs.Warning($"Hit max file limit {MaxFiles}, while more files are found");
					break;
				}

				appender.Add(
					new PpRecord([
						//
						("rec__uuid", Guid.CreateVersion7()),
						("file", FileInfoToJsonObject(path)),
					])
				);
			}
		}

		await OutFiles.BaseTable.DoneAsync();
	}

	private static string FileInfoToJsonObject(string path)
	{
		var fi = new FileInfo(path);

		return PpJson.SerializeToString(
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
		);
	}
}
