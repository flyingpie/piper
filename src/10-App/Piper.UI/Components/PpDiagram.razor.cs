using Blazor.Diagrams;
using Microsoft.AspNetCore.Components;
using Piper.Core.Nodes;
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
					new PpListFilesNode()
					{
						Position = new BD.Point(args.ClientX, args.ClientY), Name = "Node 3",
						// InPath = "",
						// InPattern = "*.pfx",
						// Command = "cat",
						// Args =
						// [
						// 	new ListFilesNodeModel.CmdArgument()
						// 	{
						// 		Arg = "/home/marco/Downloads/jsonnd.txt",
						// 	},
						// ],
					});
				break;

			case "PP_NODE_READ_FILES":
				var node = Diagram.Nodes.Add(
					new PpCatFilesNode()
					{
						Name = "Node 3",
						Position = new BD.Point(args.ClientX, args.ClientY),
					});
				break;

			default:
				Console.WriteLine($"Unknown context value '{args.Value}'");
				break;
		}

		ContextMenuService.Close();
	}
}