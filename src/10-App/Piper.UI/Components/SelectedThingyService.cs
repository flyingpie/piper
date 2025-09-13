using Piper.Core;

namespace Piper.UI.Components;

public class SelectedThingyService
{
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

	public void OnChanged(Action onChanged)
	{
		_onChanged.Add(onChanged);
	}
}
