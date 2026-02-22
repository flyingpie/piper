using Blazor.Diagrams;
using Microsoft.AspNetCore.Components;
using Piper.UI.Services;

namespace Piper.UI.Pages.Welcome;

public partial class Main
{
	private int _tabIdxBottom;
	private int _tabIdxSide;

	[Inject]
	private BlazorDiagram? Diagram { get; set; } = null!;

	protected override void OnInitialized()
	{
		SelectedThingyService.Instance.OnSelectedPort(() => {
			// _tab
		});
	}
}
