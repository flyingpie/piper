using Microsoft.AspNetCore.Components;
using Piper.Core.Nodes;
using Piper.UI.Services;

namespace Piper.UI.Components.Nodes;

public partial class NodeView<TNodeType> : ComponentBase
	where TNodeType : class, IPpNode
{
	[Inject]
	public SelectedThingyService? SelectedThingy { get; set; }

	[EditorRequired]
	[Parameter]
	public RenderFragment ChildContent { get; set; } = null!;

	[EditorRequired]
	[Parameter]
	public TNodeType Node { get; set; } = null!;

	protected override Task OnInitializedAsync()
	{
		SelectedThingy.OnChanged(() => InvokeAsync(() => StateHasChanged()));

		return Task.CompletedTask;
	}

	protected override Task OnParametersSetAsync()
	{
		return base.OnInitializedAsync();
	}
}