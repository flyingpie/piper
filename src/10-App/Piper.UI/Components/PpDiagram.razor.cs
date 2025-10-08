using Blazor.Diagrams;
using Microsoft.AspNetCore.Components;
using Piper.Core.Nodes;
using Piper.UI.Components.Nodes;
using Radzen;
using BD = Blazor.Diagrams.Core.Geometry;

namespace Piper.UI.Components;

public partial class PpDiagram : ComponentBase
{
	[Inject]
	private BlazorDiagram? Diagram { get; set; }

	private void OnMenuItemClick(MenuItemEventArgs args)
	{
		Console.WriteLine($"VALUE:{args.Value}");

		switch (args.Value ?? string.Empty)
		{
			case "PP_NODE_LIST_FILES":
				var catNode = Diagram.Nodes.Add(
					new GenericNodeModel<PpListFilesNode>(new() { Name = "Node 3", InPath = "/home/marco/Downloads", InPattern = "*.txt", }) { Position = new BD.Point(args.ClientX, args.ClientY), });
				break;

			case "PP_NODE_READ_FILES":
				// var node = Diagram.Nodes.Add(
				// 	new PpReadFilesNode()
				// 	{
				// 		Name = "Node 3",
				// 		Position = new BD.Point(args.ClientX, args.ClientY),
				// 	});
				break;

			default:
				Console.WriteLine($"Unknown context value '{args.Value}'");
				break;
		}

		ContextMenuService.Close();
	}
}