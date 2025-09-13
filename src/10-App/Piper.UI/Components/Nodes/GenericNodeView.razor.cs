using Microsoft.AspNetCore.Components;
using Piper.Core.Nodes;
using Piper.UI.Services;

namespace Piper.UI.Components.Nodes;

public partial class GenericNodeView : ComponentBase
{
	[Parameter]
	public PpListFilesNode Node { get; set; } = null!;

	[Inject]
	public SelectedThingyService? SelectedThingy { get; set; }

	protected override Task OnInitializedAsync()
	{
		SelectedThingy.OnChanged(() => InvokeAsync(() => StateHasChanged()));

		return Task.CompletedTask;
	}

	protected override Task OnParametersSetAsync()
	{
		return base.OnInitializedAsync();
	}

	private void OnNodeMouseDown()
	{
		SelectedThingy.SelectNode(Node);
	}

	// public List<>
}