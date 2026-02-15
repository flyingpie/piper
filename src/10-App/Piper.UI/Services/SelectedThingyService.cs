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
	private Piper.Core.Data.PpNodePort? _selectedPort2;

	public PpNode? SelectedNode
	{
		get => _selectedNode;
		set
		{
			_selectedNode = value;
			Changed();
		}
	}

	// public PpNodePort? SelectedPort
	// {
	// 	get => _selectedPort;
	// 	set
	// 	{
	// 		_selectedPort = value;
	// 		Changed();
	// 	}
	// }

	public Piper.Core.Data.PpNodePort? SelectedPort2
	{
		get => _selectedPort2;
		set
		{
			_selectedPort2 = value;
			Changed();
		}
	}

	// public IPpTable? SelectedTable => SelectedPort2?.Table;

	public bool IsNodeSelected(PpNode? node) => _selectedNode != null && _selectedNode == node;

	public bool IsNodePortSelected(PpNodePort port) => _selectedPort != null && _selectedPort == port;

	public void SelectNode(PpNode? node)
	{
		SelectedNode = node;

		Changed();
	}

	// public void SelectPort(PpNodePort port)
	// {
	// 	SelectedPort = port;
	//
	// 	Changed();
	// }

	public void SelectPort2(Piper.Core.Data.PpNodePort port)
	{
		_selectedPort2 = port;

		Changed();
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
