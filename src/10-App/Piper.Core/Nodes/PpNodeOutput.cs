namespace Piper.Core.Nodes;

public class PpNodeOutput
{
	public string NodePortName { get; set; }

	public Func<PpDataFrame> DataFrame { get; set; }


	public PpTable Table { get; set; }
}