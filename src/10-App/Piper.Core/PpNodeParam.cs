namespace Piper.Core;

public enum PpParamHint
{
	None = 0,

	Code,
}

public class PpNodeParam : IPpNodeProperty
{
	public string Name { get; set; }

	// private object? _value;

	public PpParamHint Hint { get; set; }

	public object? Value
	{
		get => Getter();
		set
		{
			// _value = value;
			Setter?.Invoke(value);
		}
	}

	public Func<object?> Getter { get; set; }

	public Action<object?> Setter { get; set; }

	public int ValueAsInt
	{
		get => Value as int? ?? 0;
		set
		{
			Value = value;
			// Setter?.Invoke(this);
		}
	}

	public string ValueAsString
	{
		get => Value as string ?? string.Empty;
		set
		{
			Value = value;
			// Setter?.Invoke(this);
		}
	}
}