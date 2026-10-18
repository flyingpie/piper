namespace Piper.Core.Nodes;

public class PpStubNode : PpNode
{
	public override bool SupportsProgress => false;

	protected override Task OnExecuteAsync() => Task.CompletedTask;
}
