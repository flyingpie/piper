using Blazor.Diagrams;
using Blazor.Diagrams.Core.PathGenerators;
using Blazor.Diagrams.Core.Routers;
using Blazor.Diagrams.Options;
using Microsoft.Extensions.DependencyInjection;
using Piper.Core.Nodes;
using Piper.UI.Components.Nodes;
using Piper.UI.Services;
using BD = Blazor.Diagrams.Core.Geometry;

namespace Piper.UI;

public static class BlazorDiagramConfiguration
{
	public static IServiceCollection AddBlazorDiagram(this IServiceCollection services)
	{
		return services
			.AddSingleton<SelectedThingyService>()
			.AddSingleton(p => CreateDiagram(p.GetRequiredService<SelectedThingyService>()));
	}

	public static BlazorDiagram CreateDiagram(SelectedThingyService sel)
	{
		var options = new BlazorDiagramOptions
		{
			AllowPanning = true,
			AllowMultiSelection = true,
			Links =
			{
				DefaultColor = "#ffffff",
				DefaultPathGenerator = new SmoothPathGenerator(),
				DefaultRouter = new NormalRouter(),
				EnableSnapping = true,
				SnappingRadius = 15,
			},
			Zoom =
			{
				Enabled = true,
			},
		};

		var diagram = new BlazorDiagram(options);
		diagram.RegisterComponent<PpListFilesNode, ListFilesNodeView>();
		diagram.RegisterComponent<PpCatFilesNode, ReadFilesNodeView>();

		var catNode = diagram.Nodes.Add(
			new PpListFilesNode()
			{
				Position = new BD.Point(50, 200),
				Name = "Node 3",
				InPath = "/home/marco/Downloads",
				InPattern = "*.pfx",
				// Command = "cat",
				// Args =
				// [
				// 	new ListFilesNodeModel.CmdArgument()
				// 	{
				// 		Arg = "/home/marco/Downloads/jsonnd.txt",
				// 	},
				// ],
			}
		);

		var catNode2 = diagram.Nodes.Add(
			new PpListFilesNode()
			{
				Position = new BD.Point(400, 200),
				Name = "Node 3",
				InPath = "/home/marco/Downloads",
				InPattern = "*.pfx",
				// Command = "cat",
				// Args =
				// [
				// 	new ListFilesNodeModel.CmdArgument()
				// 	{
				// 		Arg = "/home/marco/Downloads/jsonnd.txt",
				// 	},
				// ],
			}
		);

		// var jqNode = diagram.Nodes.Add(
		// 	new ListFilesNodeModel()
		// 	{
		// 		Position = new BD.Point(400, 200),
		// 		Name = "Node 3",
		// 		Command = "jq",
		// 		Args = [new ListFilesNodeModel.CmdArgument() { Arg = "-c" }],
		// 	}
		// );

		return diagram;
	}
}