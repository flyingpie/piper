using Microsoft.AspNetCore.Components;
using Piper.UI.Components.Nodes;
using Piper.UI.Services;

namespace Piper.UI.Components;

public partial class PiperNodePort : ComponentBase
{
	[EditorRequired]
	[NotNull]
	[Parameter]
	public MyPortModel Port { get; set; } = null!;

	private bool IsPortSelected => SelectedThingyService.Instance.IsNodePortSelected(Port);

	public void OnClickShowData()
	{
		SelectedThingyService.Instance.SelectPort(Port);
	}
}