namespace Piper.Core.Data;

public class PpId
{
	private int _i;
	private int _j;

	public static PpId Instance { get; } = new();

	// public string Next() => $"{(char)_i++}{_j++}";
	public string Next() => $"t{Guid.CreateVersion7().ToString().ToLowerInvariant().Replace("-", "")}";
}
