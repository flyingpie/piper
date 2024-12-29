using Blazor.Diagrams;
using Microsoft.AspNetCore.Components;

namespace Piper.UI.Pages.Welcome;

public partial class MyDiagram : ComponentBase
{
	[Inject]
	private BlazorDiagram Diagram { get; set; }

	protected override void OnInitialized() { }
}
