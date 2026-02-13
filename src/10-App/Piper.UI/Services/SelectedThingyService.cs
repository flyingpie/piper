using Piper.Core;
using Piper.Core.Data;
using PpNodePort = Piper.Core.PpNodePort;

namespace Piper.UI.Services;

public class SelectedThingyService
{
	public static SelectedThingyService Instance { get; } = new();

	private readonly List<Action> _onChanged = [];

	private PpNode? _selectedNode;
	private PpNodePort? _selectedPort;

	public PpNode? SelectedNode
	{
		get => _selectedNode;
		set
		{
			_selectedNode = value;
			Changed();
		}
	}

	public PpNodePort? SelectedPort
	{
		get => _selectedPort;
		set
		{
			_selectedPort = value;
			Changed();
		}
	}

	public PpTable? SelectedTable => SelectedPort?.Table;

	public bool IsNodeSelected(PpNode? node) => _selectedNode != null && _selectedNode == node;

	public bool IsNodePortSelected(PpNodePort port) => _selectedPort != null && _selectedPort == port;

	public void SelectNode(PpNode? node)
	{
		SelectedNode = node;

		Changed();
	}

	public void SelectPort(PpNodePort port)
	{
		SelectedPort = port;
	}

	public void OnChanged(Action onChanged)
	{
		_onChanged.Add(onChanged);
	}

	public void Changed()
	{
		Console.WriteLine($"Changed ({_onChanged.Count})");

		foreach (var c in _onChanged)
		{
			c.Invoke();
		}
	}
}
