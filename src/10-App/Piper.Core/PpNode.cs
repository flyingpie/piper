using Blazor.Diagrams.Core.Models;
using Piper.Core.Data;
using Piper.Core.Logging;

namespace Piper.Core;

public class PpStubNode : PpNode
{
	public override bool SupportsProgress => false;

	protected override Task OnExecuteAsync() => Task.CompletedTask;
}

public abstract class PpNode : NodeModel
{
	private readonly List<IPpNodeProperty> _fixedNodeProps;

	private Action<PpNode> _onChange = _ => { };

	protected readonly List<IPpNodeProperty> _dynNodeProps = [];

	protected PpNode()
	{
		_fixedNodeProps = this.GetNodeProps().ToList();
		NodeParams = NodeProps.OfType<PpNodeParam>().ToList();
		NodePorts = NodeProps.OfType<PpNodePort>().ToList();
		Size = new(350, 300);
	}

	public string NodeId { get; set; } = PpId.Instance.NextNode();

	public virtual string NodeType => GetType().Name;

	public string Name { get; set; } = "Node";

	public virtual string Icon { get; } = "fa-solid fa-circle-nodes";

	public virtual string Color { get; } = "#2a3c68";

	public bool IsExecuting { get; set; }

	public abstract bool SupportsProgress { get; }

	public double Progress { get; set; } // 0-1

	private readonly Stopwatch _sw = new();

	public TimeSpan? Duration => _sw.ElapsedMilliseconds <= 0 ? null : _sw.Elapsed;

	public PpLogs Logs { get; } = new();

	public IEnumerable<IPpNodeProperty> NodeProps => _fixedNodeProps.Concat(_dynNodeProps);

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

		_sw.Restart();

		IsExecuting = true;

		try
		{
			// await OnExecuteAsync();

			foreach (var port in NodePorts)
			{
				var nodeInput = port.GetNodeInput?.Invoke();
				if (nodeInput == null)
				{
					continue;
				}

				await nodeInput.Modifiers.ExecuteAsync();
			}

			await Task.Run(OnExecuteAsync);

			foreach (var port in NodePorts)
			{
				var nodeOutput = port.GetNodeOutput?.Invoke();
				if (nodeOutput == null)
				{
					continue;
				}

				await nodeOutput.Modifiers.ExecuteAsync();
			}
		}
		catch (Exception ex)
		{
			Logs.Error($"Error executing node '{GetType().FullName}': {ex.Message}");
		}

		// Duration = sw.Elapsed;
		_sw.Stop();

		IsExecuting = false;

		Logs.Info($"Executed node '{GetType().FullName}', took {_sw.Elapsed}");
	}

	protected abstract Task OnExecuteAsync();
}
