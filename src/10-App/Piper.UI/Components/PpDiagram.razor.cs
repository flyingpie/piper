using Blazor.Diagrams;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Piper.Core.Nodes;
using Piper.Core.Nodes.Unix;
using Radzen;
using BD = Blazor.Diagrams.Core.Geometry;

namespace Piper.UI.Components;

public partial class PpDiagram : ComponentBase
{
	private int _idx;

	[Inject]
	private ContextMenuService ContextMenuService { get; set; } = null!;

	[Inject]
	private BlazorDiagram? Diagram { get; set; } = null!;

	private void OnMenuItemClick(MouseEventArgs a1, MenuItemEventArgs args)
	{
		if (Diagram is null)
		{
			return;
		}

		if (args.Value?.ToString()?.Equals("child", StringComparison.OrdinalIgnoreCase) ?? false)
		{
			return;
		}

		Console.WriteLine($"VALUE:{args.Value}");

		switch (args.Value ?? string.Empty)
		{
			case "PP_NODE_LIST_FILES":
				Diagram.Nodes.Add(
					new PpListFilesNode()
					{
						Name = $"List Files {++_idx:00}",
						InPath = "/home/marco/Downloads",
						InPattern = "*.txt",
						Position = new BD.Point(a1.ClientX, a1.ClientY),
					}
				);
				break;

			case "PP_NODE_READ_FILES":
				Diagram.Nodes.Add(
					new PpReadFilesNode()
					{
						Name = $"Read Files {++_idx:00}",
						InAttr = "path",
						Position = new BD.Point(a1.ClientX, a1.ClientY),
					}
				);
				// var node = Diagram.Nodes.Add(
				// 	new PpReadFilesNode()
				// 	{
				// 		Name = "Node 3",
				// 		Position = new BD.Point(args.ClientX, args.ClientY),
				// 	});
				break;

			case "PP_NODE_READ_CSV":
				Diagram.Nodes.Add(
					new PpReadCsvNode()
					{
						Name = $"Read CSV {++_idx:00}",
						// PathPattern = "/home/marco/Downloads/**/*.csv",
						Position = new BD.Point(a1.ClientX, a1.ClientY),
					}
				);
				break;

			case "PP_NODE_READ_XSLX":
				Diagram.Nodes.Add(
					new PpReadXlsxNode() { Name = $"Read XSLX {++_idx:00}", Position = new BD.Point(a1.ClientX, a1.ClientY) }
				);
				break;

			case "PP_NODE_QUERY":
				Diagram.Nodes.Add(
					new PpDuckNode()
					{
						Name = $"List Files {++_idx:00}",
						// InPath = "/home/marco/Downloads",
						// InPattern = "*.txt",
						Position = new BD.Point(a1.ClientX, a1.ClientY),
					}
				);
				break;

			case "PP_NODE_SMAP":
				Diagram.Nodes.Add(
					new PpSMapNode()
					{
						Name = $"SMAP {++_idx:00}",
						// PathPattern = "/home/marco/Downloads/**/*.csv",
						Position = new BD.Point(a1.ClientX, a1.ClientY),
					}
				);
				break;

			case "PP_NODE_RDBMS":
				Diagram.Nodes.Add(
					new PpRdbmsNode()
					{
						//
						Name = $"RDBMS {++_idx:00}",
						Position = new BD.Point(a1.ClientX, a1.ClientY),
					}
				);
				break;

			case "PP_NODE_SCRIPT":
				Diagram.Nodes.Add(
					new PpCSharpNode()
					{
						//
						Name = $"Script {++_idx:00}",
						Position = new BD.Point(a1.ClientX, a1.ClientY),
					}
				);
				break;

			case "PP_NODE_FUNCTION":
				Diagram.Nodes.Add(
					new PpFunctionNode()
					{
						//
						Name = $"Function {++_idx:00}",
						Position = new BD.Point(a1.ClientX, a1.ClientY),
					}
				);
				break;

			case "PP_NODE_RAZOR":
				Diagram.Nodes.Add(
					new PpRazorNode()
					{
						//
						Name = $"Razor {++_idx:00}",
						Position = new BD.Point(a1.ClientX, a1.ClientY),
					}
				);
				break;

			case "PP_NODE_HTTP":
				Diagram.Nodes.Add(
					new PpHttpNode()
					{
						//
						Name = $"HTTP {++_idx:00}",
						Position = new BD.Point(a1.ClientX, a1.ClientY),
					}
				);
				break;

			default:
				Console.WriteLine($"Unknown context value '{args.Value}'");
				break;
		}

		ContextMenuService.Close();
	}
}
