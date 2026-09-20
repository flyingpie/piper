using Microsoft.AspNetCore.Components;
using Piper.UI.Services;

namespace Piper.UI.Components.Nodes;

public partial class PpNodePort : ComponentBase
{
	[EditorRequired]
	[Parameter]
	public Core.PpNodePort NodePort { get; set; } = null!;

	private bool IsPortSelected => SelectedThingyService.Instance.IsNodePortSelected(NodePort.Port);

	protected override void OnParametersSet()
	{
		NodePort.OnChange(_ => InvokeAsync(StateHasChanged));
	}

	public void OnClickDisconnectAll()
	{
		NodePort.DisconnectAll();
	}

	public void OnClickModifiers()
	{
		OnClickShowData();
	}

	public void OnClickShowData()
	{
		// Input
		var inp = NodePort?.GetNodeInput?.Invoke();
		if (inp != null)
		{
			SelectedThingyService.Instance.SelectPort(inp);
		}

		// Output
		var outp = NodePort?.GetNodeOutput?.Invoke();
		if (outp != null)
		{
			SelectedThingyService.Instance.SelectPort(outp);
		}
	}
}
