namespace Piper.Core.Nodes;

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