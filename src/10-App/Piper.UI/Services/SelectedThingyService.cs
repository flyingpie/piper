using Piper.Core;

namespace Piper.UI.Services;

public class SelectedThingyService
{
	public static SelectedThingyService Instance { get; } = new();

	private readonly List<Action> _onSelectedNode = [];
	private readonly List<Action> _onSelectedPort = [];

	private PpNode? _selectedNode;
	private Piper.Core.Data.PpNodePort? _selectedPort;

	public PpNode? SelectedNode => _selectedNode;

	public Piper.Core.Data.PpNodePort? SelectedPort => _selectedPort;

	public void OnSelectedNode(Action action) => _onSelectedNode.Add(action);

	public void OnSelectedPort(Action action) => _onSelectedPort.Add(action);

	public bool IsNodeSelected(PpNode? node) => _selectedNode != null && _selectedNode == node;

	public bool IsNodePortSelected(Piper.Core.Data.PpNodePort port) => _selectedPort != null && _selectedPort == port;

	public void SelectNode(PpNode? node)
	{
		_selectedNode = node;

		OnSelectedNode();
	}

	public void SelectPort(Piper.Core.Data.PpNodePort? port)
	{
		_selectedPort = port;

		OnSelectedNode();
	}

	private void OnSelectedNode()
	{
		foreach (var c in _onSelectedNode)
		{
			c.Invoke();
		}
	}

	private void OnSelectedPort()
	{
		foreach (var c in _onSelectedPort)
		{
			c.Invoke();
		}
	}
}
