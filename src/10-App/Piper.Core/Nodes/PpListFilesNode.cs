namespace Piper.Core.Nodes;

public enum PpPortDirection
{
	In,
	Out,
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class PpPortAttribute(PpPortDirection direction) : Attribute
{
	public PpPortDirection Direction { get; } = direction;
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class PpInput(string name) : Attribute
{
	public string Name { get; } = Guard.Against.NullOrWhiteSpace(name);
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class PpOutput(string name) : Attribute
{
	public string Name { get; } = Guard.Against.NullOrWhiteSpace(name);
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class PpParam(string name) : Attribute
{
	public string Name { get; } = Guard.Against.NullOrWhiteSpace(name);
}

public class PpListFilesNode : IPpNode
{
	private PpDataFrame _files = new();

	public bool IsExecuting { get; set; }

	public string NodeType => "List Files";

	public string? Name { get; set; }

	[PpParam("Path")]
	public string InPath { get; set; }

	[PpParam("Pattern")]
	public string InPattern { get; set; }

	[PpOutput("Files")]
	public PpNodeOutput OutFiles { get; } = new();

	public async Task ExecuteAsync()
	{
		_files.Records.Clear();

		var dirs = Directory.GetFiles(
			path: InPath,
			searchPattern: InPattern,
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
					},
				})
				.ToList(),
		};

		OutFiles.DataFrame = () => _files;
	}
}