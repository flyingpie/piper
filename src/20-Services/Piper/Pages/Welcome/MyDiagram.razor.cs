using Blazor.Diagrams;
using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.PathGenerators;
using Blazor.Diagrams.Core.Routers;
using Blazor.Diagrams.Options;
using Microsoft.AspNetCore.Components;
using Piper.Components.Nodes;

namespace Piper.Pages.Welcome;

public partial class MyDiagram : ComponentBase
{
	[Inject]
	private BlazorDiagram Diagram { get; set; }

	protected override void OnInitialized()
	{
	}
}