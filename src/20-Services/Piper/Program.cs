using Blazor.Diagrams;
using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.PathGenerators;
using Blazor.Diagrams.Core.Routers;
using Blazor.Diagrams.Options;
using Microsoft.Extensions.DependencyInjection;
using Photino.Blazor;
using Piper.Components;
using Piper.Components.Nodes;

namespace Piper;

public static class Program
{
	public static async Task Main(string[] args)
	{
		var appBuilder = PhotinoBlazorAppBuilder.CreateDefault();

		// TODO: Unify with the main app DI.
		appBuilder.Services
			.AddUI()
			.AddLogging()
			.AddSingleton(p => CreateDiagram(p.GetRequiredService<SelectedThingyService>()))
			.AddSingleton<SelectedThingyService>();

		appBuilder.RootComponents.Add<App>("app");

		var _app = appBuilder.Build();

		_app.MainWindow
			.SetLogVerbosity(0)
			.SetSize(new System.Drawing.Size(1920, 1080))
			// .SetIconFile(WtqPaths.GetPathRelativeToWtqAppDir("assets", "icon-v2-64.png"))
			.SetTitle("Piper");

//		_app.MainWindow.RegisterWindowCreatedHandler((s, a) => { _ = Task.Run(CloseMainWindowAsync); });

		_app.Run();
	}

	public static BlazorDiagram CreateDiagram(SelectedThingyService sel)
	{
		var options = new BlazorDiagramOptions
		{
			AllowMultiSelection = true,
			Zoom =
			{
				Enabled = false,
			},
			Links =
			{
				DefaultRouter = new NormalRouter(),
				DefaultPathGenerator = new SmoothPathGenerator(),
			},
		};

		var Diagram = new BlazorDiagram(options);
		Diagram.RegisterComponent<ListFilesNodeModel, ListFilesNode>();

		var firstNode = Diagram.Nodes.Add(new NodeModel(position: new Point(50, 50)) { Title = "Node 1", });

		var secondNode = Diagram.Nodes.Add(new NodeModel(position: new Point(200, 100)) { Title = "Node 2", });

		var catNode = Diagram.Nodes.Add(new ListFilesNodeModel(sel, position: new Point(50, 200))
		{
			Title = "Node 3",
			Command = "cat",
			Args = "/home/marco/Downloads/jsonnd.txt",
		});

		var jqNode = Diagram.Nodes.Add(new ListFilesNodeModel(sel, position: new Point(400, 200))
		{
			Title = "Node 3",
			Command = "jq",
			Args = "-c",
		});

		var src = new SinglePortAnchor(catNode.StdOut);
		var dst = new SinglePortAnchor(jqNode.StdIn);
		var link1 = Diagram.Links.Add(new LinkModel(src, dst));



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
		var link = Diagram.Links.Add(new LinkModel(sourceAnchor, targetAnchor));

		link.Color = "#aaaaaa";
		link.PathGenerator = new StraightPathGenerator();
		link.Router = new OrthogonalRouter();
		link.Width = 2;

		// link.Segmentable = true;

		return Diagram;
	}
}