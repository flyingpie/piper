using Blazor.Diagrams;
using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.PathGenerators;
using Blazor.Diagrams.Core.Routers;
using Blazor.Diagrams.Options;
using Microsoft.Extensions.DependencyInjection;
using Piper.UI.Components;
using Piper.UI.Components.Nodes;

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
			AllowMultiSelection = true,
			Zoom = { Enabled = false },
			Links =
			{
				DefaultRouter = new NormalRouter(),
				DefaultPathGenerator = new SmoothPathGenerator(),
			},
		};

		var diagram = new BlazorDiagram(options);
		diagram.RegisterComponent<ListFilesNodeModel, ListFilesNode>();

		var firstNode = diagram.Nodes.Add(
			new NodeModel(position: new Point(50, 50)) { Title = "Node 1" }
		);

		var secondNode = diagram.Nodes.Add(
			new NodeModel(position: new Point(200, 100)) { Title = "Node 2" }
		);

		var catNode = diagram.Nodes.Add(
			new ListFilesNodeModel(position: new Point(50, 200))
			{
				Title = "Node 3",
				Command = "cat",
				Args = [ new ListFilesNodeModel.CmdArgument() { Arg = "/home/marco/Downloads/jsonnd.txt" }],
			}
		);

		var jqNode = diagram.Nodes.Add(
			new ListFilesNodeModel(position: new Point(400, 200))
			{
				Title = "Node 3",
				Command = "jq",
				Args = [ new ListFilesNodeModel.CmdArgument() { Arg = "-c" }],
			}
		);

		var src = new SinglePortAnchor(catNode.StdOut);
		var dst = new SinglePortAnchor(jqNode.StdIn);
		var link1 = diagram.Links.Add(new LinkModel(src, dst));

		var p1 = firstNode.AddPort(PortAlignment.Right);
		var p2 = firstNode.AddPort(PortAlignment.Right);
		// p2.Position = new Point(0, 40);

		var leftPort = secondNode.AddPort(PortAlignment.Left);
		var rightPort = secondNode.AddPort(PortAlignment.Right);

		// The connection point will be the intersection of
		// a line going from the target to the center of the source
		// var sourceAnchor = new ShapeIntersectionAnchor(firstNode);
		var sourceAnchor = new SinglePortAnchor(p1);

		// The connection point will be the port's position
		var targetAnchor = new SinglePortAnchor(leftPort);
		var link = diagram.Links.Add(new LinkModel(sourceAnchor, targetAnchor));

		link.Color = "#aaaaaa";
		link.PathGenerator = new StraightPathGenerator();
		link.Router = new OrthogonalRouter();
		link.Width = 2;

		// link.Segmentable = true;

		return diagram;
	}
}
