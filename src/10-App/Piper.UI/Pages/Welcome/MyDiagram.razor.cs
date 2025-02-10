using Blazor.Diagrams;
using Microsoft.AspNetCore.Components;
using Piper.Core;

namespace Piper.UI.Pages.Welcome;

public partial class MyDiagram : ComponentBase
{
	[Inject]
	private BlazorDiagram? Diagram { get; set; }

	[Inject]
	private SaveLoadService? SaveLoad { get; set; }

	protected override void OnInitialized()
	{
		// Method intentionally left empty.
	}
}
