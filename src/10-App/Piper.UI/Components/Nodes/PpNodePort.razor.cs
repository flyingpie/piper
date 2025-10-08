using Microsoft.AspNetCore.Components;
using Piper.UI.Services;

namespace Piper.UI.Components.Nodes;

public partial class PpNodePort : ComponentBase
{
	[EditorRequired]
	[Parameter]
	public MyPortModel Port { get; set; } = null!;

	private bool IsPortSelected => SelectedThingyService.Instance.IsNodePortSelected(Port);

	public void OnClickShowData()
	{
		SelectedThingyService.Instance.SelectPort(Port);
	}
}