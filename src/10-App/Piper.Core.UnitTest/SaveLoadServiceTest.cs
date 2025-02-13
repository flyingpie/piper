// using Blazor.Diagrams;
// using Blazor.Diagrams.Core.Anchors;
// using Blazor.Diagrams.Core.Geometry;
// using Blazor.Diagrams.Core.Models;
// using Blazor.Diagrams.Core.PathGenerators;
// using Blazor.Diagrams.Core.Routers;
// using Blazor.Diagrams.Options;
// using Microsoft.VisualStudio.TestTools.UnitTesting;
// using Piper.UI.Components.Nodes;
//
// namespace Piper.Core.UnitTest;
//
// [TestClass]
// public class SaveLoadServiceTest
// {
// 	[TestMethod]
// 	public void TestMethod1()
// 	{
// 		var d = CreateDiagram();
//
// 		var svc = new SaveLoadService();
//
// 		svc.Save(d);
//
// 		var dbg = 2;
// 	}
//
// 	public PiperGraph CreateDiagram()
// 	{
// 		var diagram = new PiperGraph();
//
// 		diagram.RegisterComponent<ListFilesNodeModel, UI.Components.Nodes.ListFilesNode>();
//
// 		var catNode = diagram.Nodes.Add(
// 			new ListFilesNodeModel()
// 			{
// 				Position = new Point(50, 200),
// 				Title = "Node 3",
// 				Command = "cat",
// 				Args =
// 				[
// 					new ListFilesNodeModel.CmdArgument()
// 					{
// 						Arg = "/home/marco/Downloads/jsonnd.txt",
// 					},
// 				],
// 			}
// 		);
//
// 		var jqNode = diagram.Nodes.Add(
// 			new ListFilesNodeModel()
// 			{
// 				Position = new Point(400, 200),
// 				Title = "Node 3",
// 				Command = "jq",
// 				Args = [new ListFilesNodeModel.CmdArgument() { Arg = "-c" }],
// 			}
// 		);
//
// 		var src = new SinglePortAnchor(catNode.StdOut);
// 		var dst = new SinglePortAnchor(jqNode.StdIn);
// 		var link1 = diagram.Links.Add(new LinkModel(src, dst));
//
// 		return diagram;
// 	}
// }
