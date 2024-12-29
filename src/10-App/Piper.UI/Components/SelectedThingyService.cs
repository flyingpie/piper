namespace Piper.UI.Components;

public class SelectedThingyService
{
	private PiperDataFrame _node;

	public PiperDataFrame? Node
	{
		get => _node;
		set
		{
			_node = value;
			OnChanged?.Invoke();
		}
	}

	public Action OnChanged { get; set; }
}
