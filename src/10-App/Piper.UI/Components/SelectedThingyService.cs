using System.Collections.Generic;
using Piper.Core;
using Piper.Core.Data;

namespace Piper.UI.Components;

public class SelectedThingyService
{
	private PiperDataFrame? _node;

	public PiperDataFrame? Node
	{
		get => _node;
		set
		{
			_node = value;
			// OnChanged?.Invoke();
			Changed();
		}
	}

	// public Action OnChanged { get; set; }

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
