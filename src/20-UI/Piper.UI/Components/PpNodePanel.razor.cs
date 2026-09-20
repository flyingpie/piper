using Microsoft.AspNetCore.Components;
using Piper.Core;
using Piper.UI.Services;

namespace Piper.UI.Components;

public partial class PpNodePanel : ComponentBase
{
	// [Parameter]
	public PpNode? SelectedNode => SelectedThingyService.Instance.SelectedNode; //{ get; set; }

	protected override Task OnInitializedAsync()
	{
		SelectedThingyService.Instance.OnSelectedNode(() => InvokeAsync(StateHasChanged));

		return Task.CompletedTask;
	}
}
