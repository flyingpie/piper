namespace Piper.Core.Nodes;

public interface IPpNode
{
	string NodeType { get; }

	string Name { get; set; }

	Task ExecuteAsync();
}