using Piper.Core.Nodes;
using Piper.UI.Components.Nodes;

namespace Piper.UI.Services;

public class SelectedThingyService
{
	public static SelectedThingyService Instance { get; } = new();

	private readonly List<Action> _onChanged = [];
	private IPpNode? _selectedNode;
	private MyPortModel? _selectedPort;

	public MyPortModel? SelectedPort
	{
		get => _selectedPort;
		set
		{
			_selectedPort = value;
			Changed();
		}
	}

	public IPpNode? SelectedNode
	{
		get => _selectedNode;
		set
		{
			_selectedNode = value;
			Changed();
		}
	}

	public void Changed()
	{
		Console.WriteLine($"Changed ({_onChanged.Count})");

		foreach (var c in _onChanged)
		{
			c.Invoke();
		}
	}

	public bool IsNodeSelected(IPpNode? node) => _selectedNode != null && _selectedNode == node;

	public bool IsNodePortSelected(MyPortModel port) => _selectedPort != null && _selectedPort == port;

	public void SelectNode(IPpNode? node)
	{
		SelectedNode = node;

		Changed();
	}

	public void SelectPort(MyPortModel port)
	{
		SelectedPort = port;
	}

	public void OnChanged(Action onChanged)
	{
		_onChanged.Add(onChanged);
	}
}