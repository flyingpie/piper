using Blazor.Diagrams.Core.Models;
using Piper.Core.Logging;
using shortid;
using shortid.Configuration;

namespace Piper.Core;

public abstract class PpNode : NodeModel
{
	protected PpNode()
	{
		NodeProps = this.GetNodeProps().ToList();
		NodeParams = NodeProps.OfType<PpNodeParam>().ToList();
		NodePorts = NodeProps.OfType<PpNodePort>().ToList();

		// foreach (var link in Links)
		// {
		//
		// }
	}

	public string NodeId { get; set; }
		= ShortId.Generate(new GenerationOptions(useNumbers: true, useSpecialCharacters: false));

	public virtual string NodeType
		=> GetType().Name;

	public string Name { get; set; }

	public bool IsExecuting { get; set; }

	public PpLogs Logs { get; } = new();

	public IReadOnlyCollection<IPpNodeProperty> NodeProps { get; }

	public IReadOnlyCollection<PpNodeParam> NodeParams { get; }

	public IReadOnlyCollection<PpNodePort> NodePorts { get; }

	public async Task ExecuteAsync()
	{
		Logs.Clear();
		Logs.Info($"Executing node '{GetType().FullName}'");

		var sw = Stopwatch.StartNew();

		IsExecuting = true;

		try
		{
			await Task.Run(
				async () =>
				{
					await OnExecuteAsync();
				});
		}
		catch (Exception ex)
		{
			Logs.Error($"Error executing node '{GetType().FullName}': {ex.Message}");
		}

		IsExecuting = false;

		Logs.Info($"Executed node '{GetType().FullName}', took {sw.Elapsed}");
	}

	protected abstract Task OnExecuteAsync();
}