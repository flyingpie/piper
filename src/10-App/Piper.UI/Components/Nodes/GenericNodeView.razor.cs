using Microsoft.AspNetCore.Components;
using Piper.Core.Nodes;
using Piper.UI.Services;

namespace Piper.UI.Components.Nodes;

public partial class GenericNodeView : ComponentBase
{
	[Parameter]
	public PpNodeBase Node { get; set; } = null!; // Note that this property _has_ to be named "Node" in order for Blazor Diagrams to bind to it.

	protected override Task OnInitializedAsync()
	{
		SelectedThingyService.Instance.OnChanged(() => InvokeAsync(() => StateHasChanged()));

		return Task.CompletedTask;
	}

	protected override Task OnParametersSetAsync()
	{
		return base.OnInitializedAsync();
	}

	private void OnNodeMouseDown()
	{
	}
}