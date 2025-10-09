using Microsoft.AspNetCore.Components;
using Piper.Core;
using Piper.UI.Services;

namespace Piper.UI.Components.Nodes;

public partial class GenericNodeView<TNode>
{
	[Parameter]
	public GenericNodeModel<TNode> Model { get; set; } = null!;

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