using Blazor.Diagrams;
using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
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
				// DefaultPathGenerator = new StraightPathGenerator(),
				DefaultRouter = new NormalRouter(),
				// DefaultRouter = new OrthogonalRouter(),
				EnableSnapping = true,
				SnappingRadius = 15,
				// Factory = (diagram1, source, targetAnchor) =>
				// {
				// 	Anchor source1;
				// 	switch (source)
				// 	{
				// 		case NodeModel model2:
				// 			source1 = (Anchor)new ShapeIntersectionAnchor(model2);
				// 			break;
				// 		case PortModel port2:
				// 			source1 = (Anchor)new SinglePortAnchor(port2);
				// 			break;
				// 		default:
				// 			throw new NotImplementedException();
				// 	}
				//
				// 	return (BaseLinkModel)new LinkModel(source1, targetAnchor);
				// },
			},
			Zoom = { Enabled = true, },
		};

		var diagram = new BlazorDiagram(options);
		diagram.RegisterComponent<GenericNodeModel<PpListFilesNode>, GenericNodeView<PpListFilesNode>>();
		diagram.RegisterComponent<GenericNodeModel<PpReadFilesNode>, GenericNodeView<PpReadFilesNode>>();
		// diagram.RegisterComponent<PpListFilesNode, ListFilesNodeView>();
		// diagram.RegisterComponent<PpCatFilesNode, ReadFilesNodeView>();

		var catNode = diagram.Nodes.Add(
			new GenericNodeModel<PpListFilesNode>(new()
				{
					Name = "Node 3",
					InPath = "/home/marco/Downloads",
					InPattern = "*.pfx",
				})
			{
				Position = new BD.Point(50, 200),

				// Command = "cat",
				// Args =
				// [
				// 	new ListFilesNodeModel.CmdArgument()
				// 	{
				// 		Arg = "/home/marco/Downloads/jsonnd.txt",
				// 	},
				// ],
			});

		var catNode2 = diagram.Nodes.Add(
			new GenericNodeModel<PpReadFilesNode>(new()
			{
				Name = "Node 3",
				// InPath = "/home/marco/Downloads",
				// InPattern = "*.pfx",
			})
			{
				Position = new BD.Point(400, 200),

				// Command = "cat",
				// Args =
				// [
				// 	new ListFilesNodeModel.CmdArgument()
				// 	{
				// 		Arg = "/home/marco/Downloads/jsonnd.txt",
				// 	},
				// ],
			});

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