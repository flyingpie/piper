namespace Piper.Core;

public class MyParam : INodeProperty
{
	private object? _value;

	public string Name { get; set; }

	public object? Value
	{
		get => _value;
		set
		{
			_value = value;
			OnSet?.Invoke(this);
		}
	}

	public Action<MyParam> OnSet { get; set; }

	public int ValueAsInt
	{
		get => _value as int? ?? 0;
		set
		{
			_value = value;
			OnSet?.Invoke(this);
		}
	}

	public string ValueAsString
	{
		get => _value as string ?? string.Empty;
		set
		{
			_value = value;
			OnSet?.Invoke(this);
		}
	}
}