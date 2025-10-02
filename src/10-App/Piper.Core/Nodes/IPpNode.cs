namespace Piper.Core.Nodes;

public interface IPpNode
{
	string NodeType { get; }

	string Name { get; set; }

	bool IsExecuting { get; }

	Task ExecuteAsync();
}

public static class Ext2
{
}