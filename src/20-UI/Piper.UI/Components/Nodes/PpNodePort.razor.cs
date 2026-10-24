using Microsoft.AspNetCore.Components;
using Piper.UI.Services;

namespace Piper.UI.Components.Nodes;

public partial class PpNodePort : ComponentBase
{
	[EditorRequired]
	[Parameter]
	public Core.Nodes.PpPort Port { get; set; } = null!;

	private bool IsPortSelected => SelectedThingyService.Instance.IsNodePortSelected(Port.Port);

	protected override void OnParametersSet()
	{
		Port.OnChange(_ => InvokeAsync(StateHasChanged));
	}

	public void OnClickDisconnectAll()
	{
		Port.DisconnectAll();
	}

	public void OnClickModifiers()
	{
		OnClickShowData();
	}

	public void OnClickShowData()
	{
		// Input
		var inp = Port?.GetNodeInput?.Invoke();
		if (inp != null)
		{
			SelectedThingyService.Instance.SelectPort(inp);
		}

		// Output
		var outp = Port?.GetNodeOutput?.Invoke();
		if (outp != null)
		{
			SelectedThingyService.Instance.SelectPort(outp);
		}
	}
}
