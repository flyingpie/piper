using Microsoft.AspNetCore.Components;
using Piper.UI.Services;

namespace Piper.UI.Components.Nodes;

public partial class PpNodePort : ComponentBase
{
	[EditorRequired]
	[Parameter]
	public Core.PpNodePort NodePort { get; set; } = null!;

	private bool IsPortSelected => SelectedThingyService.Instance.IsNodePortSelected(NodePort);

	protected override void OnParametersSet()
	{
		NodePort.OnChange(_ => InvokeAsync(StateHasChanged));
	}

	public void OnClickShowData()
	{
		SelectedThingyService.Instance.SelectPort(NodePort);
	}
}
