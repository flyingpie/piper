using Blazor.Diagrams;
using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.PathGenerators;
using Blazor.Diagrams.Core.Routers;
using Blazor.Diagrams.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Piper.UI.Components.Nodes;

namespace Piper.Core.UnitTest;

[TestClass]
public class SaveLoadServiceTest
{
	[TestMethod]
	public void TestMethod1()
	{
		var d = CreateDiagram();

		var svc = new SaveLoadService();

		svc.Save(d);

		var dbg = 2;
	}

	public BlazorDiagram CreateDiagram()
	{
		var diagram = new BlazorDiagram(new BlazorDiagramOptions());

		diagram.RegisterComponent<ListFilesNodeModel, ListFilesNode>();

		var firstNode = diagram.Nodes.Add(
			new NodeModel(position: new Point(50, 50)) { Title = "Node 1" }
		);

		var secondNode = diagram.Nodes.Add(
			new NodeModel(position: new Point(200, 100)) { Title = "Node 2" }
		);

		var catNode = diagram.Nodes.Add(
			new ListFilesNodeModel()
			{
				Position = new Point(50, 200),
				Title = "Node 3",
				Command = "cat",
				Args =
				[
					new ListFilesNodeModel.CmdArgument()
					{
						Arg = "/home/marco/Downloads/jsonnd.txt",
					},
				],
			}
		);

		var jqNode = diagram.Nodes.Add(
			new ListFilesNodeModel()
			{
				Position = new Point(400, 200),
				Title = "Node 3",
				Command = "jq",
				Args = [new ListFilesNodeModel.CmdArgument() { Arg = "-c" }],
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

		var sourceAnchor = new SinglePortAnchor(p1);

		var targetAnchor = new SinglePortAnchor(leftPort);
		var link = diagram.Links.Add(new LinkModel(sourceAnchor, targetAnchor));

		link.Color = "#aaaaaa";
		link.PathGenerator = new StraightPathGenerator();
		link.Router = new OrthogonalRouter();
		link.Width = 2;

		return diagram;
	}
}
