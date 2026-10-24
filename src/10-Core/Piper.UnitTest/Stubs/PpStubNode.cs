using System.Threading.Tasks;
using Piper.Core.Nodes;

namespace Piper.UnitTest.Stubs;

public class PpStubNode : PpNode
{
	public override bool SupportsProgress => false;

	protected override Task OnExecuteAsync() => Task.CompletedTask;
}
