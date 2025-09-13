using Piper.Core;
using Piper.Core.Nodes;

namespace Piper.UI.Services;

public class SelectedThingyService
{
	public IPpNode? SelectedNode { get; set; }

	private PpDataFrame? _node;
	// private List<string> _node;

	public PpDataFrame? Node
	{
		get => _node;
		set
		{
			_node = value;
			Changed();
		}
	}

	// public List<string>? Node
	// {
	// 	get => _node;
	// 	set
	// 	{
	// 		_node = value;
	// 		Changed();
	// 	}
	// }

	private readonly List<Action> _onChanged = [];

	public void Changed()
	{
		foreach (var c in _onChanged)
		{
			c.Invoke();
		}
	}

	public bool IsNodeSelected(IPpNode? node) => SelectedNode == node;

	public void OnChanged(Action onChanged)
	{
		_onChanged.Add(onChanged);
	}

	public void SelectNode(IPpNode? node)
	{
		SelectedNode = node;

		Changed();
	}
}