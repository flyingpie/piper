using Microsoft.AspNetCore.Components;
using Piper.Core;
using Piper.Core.Nodes;

namespace Piper.UI.Components.Nodes;

public abstract class NodeViewBase<TNode> : ComponentBase
	where TNode : IPpNode
{
	[Parameter]
	public GenericNodeModel<TNode> Node { get; set; } = null!;
}