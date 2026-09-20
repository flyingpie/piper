namespace Piper.Core.Data;

public class PpId
{
	private int _i;

	public static PpId Instance { get; } = new();

	public string NextMod() => $"mod{++_i:0000}";

	public string NextNode() => $"node{++_i:0000}";

	public string NextTable() => $"tbl{++_i:0000}";

	public void Reset() => _i = 0;
}
