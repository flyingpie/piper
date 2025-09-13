using Microsoft.AspNetCore.Components;
using Piper.Core.Nodes;

namespace Piper.UI.Components.Nodes;

public abstract class NodeViewBase<TNodeType> : ComponentBase
	where TNodeType : class, IPpNode
{
	[Parameter]
	public TNodeType Node { get; set; } = null!;
}