using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;
using Piper.Pages.Welcome;

namespace Piper.Components.Nodes;

public partial class ListFilesNode : ComponentBase
{
	[Parameter]
	public ListFilesNodeModel Node { get; set; } = null!;

	[Inject]
	public SelectedThingyService SelectedThingy { get; set; }

	// public PortModel Port1 { get; set; }
	//
	// public PortModel Port2 { get; set; }

	bool _init;

	protected override Task OnParametersSetAsync()
	{
		// if (!_init && Node != null)
		// {
		// 	// Port1 = new(parent: Node, PortAlignment.Right, position: new(50, 50), size: new Size(20, 20));
		// 	// Port2 = new(parent: Node, PortAlignment.Right, position: new(50, 80), size: new Size(20, 20));
		//
		// 	var Port1 = Node.AddPort(PortAlignment.Left);
		// 	var Port2 = Node.AddPort(PortAlignment.Right);
		//
		// 	_init = true;
		// }

		return base.OnInitializedAsync();
	}
}