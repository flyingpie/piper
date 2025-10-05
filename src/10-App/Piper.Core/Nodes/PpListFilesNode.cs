namespace Piper.Core.Nodes;

public class PpListFilesNode : PpNodeBase
{
	private PpTable _files = new()
	{
		TableName = "listfiles",
		Columns =
		[
			new PpColumn() { Name = "uuid" },
			new PpColumn() { Name = "path" },
			new PpColumn() { Name = "dir" },
			new PpColumn() { Name = "file" },
			new PpColumn() { Name = "ext" },
		],
	};

	public PpListFilesNode()
	{
		OutFiles = new()
		{
			Table = () => _files,
		};
	}

	public override string NodeType => "List Files";

	public override string? Name { get; set; }

	[PpParam("Path")]
	public string? InPath { get; set; }

	[PpParam("Pattern")]
	public string? InPattern { get; set; }

	[PpOutput("Files")]
	public PpNodeOutput OutFiles { get; }

	protected override async Task OnExecuteAsync()
	{
		await _files.ClearAsync();

		var records = await Task.Run(async () =>
		{
			var dirs = Directory.GetFiles(
				path: InPath,
				searchPattern: InPattern,
				new EnumerationOptions()
				{
					RecurseSubdirectories = true, // TODO: Use glob instead
				});

			return dirs
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
		});

		await _files.AddAsync(records);
	}
}