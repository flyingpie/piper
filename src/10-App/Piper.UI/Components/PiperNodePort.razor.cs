using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;
using Piper.Core;
using Piper.UI.Services;

namespace Piper.UI.Components;

public partial class PiperNodePort : ComponentBase
{
	[EditorRequired]
	[NotNull]
	[Parameter]
	public PortModel? Port { get; set; }

	// [Inject]
	// public SelectedThingyService? SelectedThingy { get; set; }

	[Parameter]
	public Func<PpDataFrame>? SelectDataFrame { get; set; }

	private bool IsPortSelected => SelectDataFrame?.Invoke() != null && SelectedThingyService.Instance.IsDataFrameSelected(SelectDataFrame?.Invoke());

	public void OnClickShowData()
	{
		var res = SelectDataFrame?.Invoke();

		SelectedThingyService.Instance.Node = res;
		// SelectedThingy.Node = ["Sup!"];
	}
}