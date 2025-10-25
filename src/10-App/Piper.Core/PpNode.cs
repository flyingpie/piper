using Blazor.Diagrams.Core.Models;
using Piper.Core.Logging;
using shortid;
using shortid.Configuration;

namespace Piper.Core;

public abstract class PpNode : NodeModel
{
	private Action<PpNode> _onChange = _ => { };

	protected PpNode()
	{
		NodeProps = this.GetNodeProps().ToList();
		NodeParams = NodeProps.OfType<PpNodeParam>().ToList();
		NodePorts = NodeProps.OfType<PpNodePort>().ToList();
	}

	public string NodeId { get; set; }
		= ShortId.Generate(new GenerationOptions(useNumbers: true, useSpecialCharacters: false));

	public virtual string NodeType
		=> GetType().Name;

	public string Name { get; set; }

	public virtual string Icon { get; } = "fa-solid fa-circle-nodes";

	public virtual string Color { get; } = "#2a3c68";

	public bool IsExecuting { get; set; }

	public abstract bool SupportsProgress { get; }

	public double Progress { get; set; } // 0-1

	public TimeSpan? Duration { get; set; }

	public PpLogs Logs { get; } = new();

	public IReadOnlyCollection<IPpNodeProperty> NodeProps { get; }

	public IReadOnlyCollection<PpNodeParam> NodeParams { get; }

	public IReadOnlyCollection<PpNodePort> NodePorts { get; }

	public void OnChange(Action<PpNode> onChange)
	{
		Guard.Against.Null(onChange);

		_onChange = onChange;
	}

	protected void Changed() => _onChange.Invoke(this);

	public async Task ExecuteAsync()
	{
		Logs.Clear();
		Logs.Info($"Executing node '{GetType().FullName}'");

		var sw = Stopwatch.StartNew();

		IsExecuting = true;

		try
		{
			// await OnExecuteAsync();

			await Task.Run(OnExecuteAsync);
		}
		catch (Exception ex)
		{
			Logs.Error($"Error executing node '{GetType().FullName}': {ex.Message}");
		}

		Duration = sw.Elapsed;

		IsExecuting = false;

		Logs.Info($"Executed node '{GetType().FullName}', took {sw.Elapsed}");
	}

	protected abstract Task OnExecuteAsync();
}