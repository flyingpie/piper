using System.Diagnostics;

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

public abstract class PpNodeBase : IPpNode
{
	public abstract string NodeType { get; }

	public abstract string Name { get; set; }

	public bool IsExecuting { get; set; }

	public async Task ExecuteAsync()
	{
		Console.WriteLine($"Executing node '{GetType().FullName}'");
		var sw = Stopwatch.StartNew();

		IsExecuting = true;

		try
		{
			await OnExecuteAsync();
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error executing node '{GetType().FullName}': {ex.Message}");
		}

		IsExecuting = false;

		Console.WriteLine($"Executed node '{GetType().FullName}', took {sw.Elapsed}");
	}

	protected abstract Task OnExecuteAsync();
}

public class PpListFilesNode : PpNodeBase
{
	// private PpDataFrame _files = new();
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

	// public bool IsExecuting { get; set; }

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