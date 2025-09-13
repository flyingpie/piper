using Blazor.Diagrams.Core.Models;

namespace Piper.Core.Nodes;

// public enum PpPortDirection
// {
// 	In,
// 	Out,
// }
//
// [AttributeUsage(AttributeTargets.Property)]
// public sealed class PpPortAttribute(PpPortDirection direction) : Attribute
// {
// 	public PpPortDirection Direction { get; } = direction;
// }
//
// [AttributeUsage(AttributeTargets.Property)]
// public sealed class PpInput(string name) : Attribute
// {
// 	public string Name { get; } = Guard.Against.NullOrWhiteSpace(name);
// }
//
// [AttributeUsage(AttributeTargets.Property)]
// public sealed class PpOutput(string name) : Attribute
// {
// 	public string Name { get; } = Guard.Against.NullOrWhiteSpace(name);
// }

public class PpListFilesNode : NodeModel, IPpNode
{
	private PpDataFrame _files = new();

	public PpListFilesNode()
	{
		InPathPort = AddPort(PortAlignment.Left);
		InPatternPort = AddPort(PortAlignment.Left);
		OutFilesPort = AddPort(PortAlignment.Right);
	}

	public bool IsExecuting { get; set; }

	public string NodeType => "List Files";

	public string? Name { get; set; }

	// [PpInput("Path")]
	public PpNodeInput InPath { get; set; } = new();

	public PortModel InPathPort { get; set; }

	// [PpInput("Pattern")]
	public PpNodeInput InPattern { get; set; } = new();

	public PortModel InPatternPort { get; set; }

	// [PpOutput("Files")]
	public PpNodeOutput OutFiles { get; } = new();

	public PortModel OutFilesPort { get; set; }

	public async Task ExecuteAsync()
	{
		_files.Records.Clear();

		var dirs = Directory.GetFiles(
			path: InPath.Value,
			searchPattern: InPattern.Value,
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