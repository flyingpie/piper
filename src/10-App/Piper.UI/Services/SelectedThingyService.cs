using Piper.Core;
using Piper.Core.Data;

namespace Piper.UI.Services;

public class SelectedThingyService
{
	public static SelectedThingyService Instance { get; } = new();

	private readonly List<Action> _onSelectedNode = [];
	private readonly List<Action> _onSelectedPort = [];
	private readonly List<Action> _onSelectedRecord = [];

	private PpNode? _selectedNode;
	private Piper.Core.Data.PpNodePort? _selectedPort;
	private PpRecord? _selectedRecord;

	public PpNode? SelectedNode => _selectedNode;

	public PpRecord? SelectedRecord => _selectedRecord;

	public Piper.Core.Data.PpNodePort? SelectedPort => _selectedPort;

	public void OnSelectedNode(Action action) => _onSelectedNode.Add(action);

	public void OnSelectedPort(Action action) => _onSelectedPort.Add(action);

	public void OnSelectedRecord(Action action) => _onSelectedRecord.Add(action);

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

		OnSelectedPort();
	}

	public void SelectRecord(PpRecord? record)
	{
		_selectedRecord = record;

		OnSelectedRecord();
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

	private void OnSelectedRecord()
	{
		foreach (var c in _onSelectedRecord)
		{
			c.Invoke();
		}
	}
}
