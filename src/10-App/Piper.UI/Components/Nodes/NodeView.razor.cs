using Microsoft.AspNetCore.Components;
using Piper.Core.Nodes;
using Piper.UI.Services;

namespace Piper.UI.Components.Nodes;

public partial class NodeView<TNode> : ComponentBase
	where TNode : IPpNode
{
	[EditorRequired]
	[Parameter]
	public RenderFragment ChildContent { get; set; } = null!;

	[EditorRequired]
	[Parameter]
	public GenericNodeModel<TNode> Node { get; set; } = null!;

	protected override Task OnInitializedAsync()
	{
		SelectedThingyService.Instance.OnChanged(() => InvokeAsync(() => StateHasChanged()));

		return Task.CompletedTask;
	}

	protected override Task OnParametersSetAsync()
	{
		return base.OnInitializedAsync();
	}
}