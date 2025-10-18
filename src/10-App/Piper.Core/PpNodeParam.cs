namespace Piper.Core;

public class PpNodeParam : IPpNodeProperty
{
	public string Name { get; set; }

	public object? Value
	{
		get => Getter();
		set
		{
			Value = value;
			Setter?.Invoke(this);
		}
	}

	public Func<object?> Getter { get; set; }

	public Action<PpNodeParam> Setter { get; set; }

	public int ValueAsInt
	{
		get => Value as int? ?? 0;
		set
		{
			Value = value;
			Setter?.Invoke(this);
		}
	}

	public string ValueAsString
	{
		get => Value as string ?? string.Empty;
		set
		{
			Value = value;
			Setter?.Invoke(this);
		}
	}
}