using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Piper.Core.Nodes;

namespace Piper.Core.UnitTest;

[TestClass]
public class MyGraphTest
{
	[TestMethod]
	public async Task METHOD()
	{
		var sw = Stopwatch.StartNew();

		var node1 = new PpListFilesNode
		{
			Name = "Node Name",
			InPath = "/home/marco/Downloads",
			// InPattern = "logs-*.txt",
			InPattern = "*.log",
		};

		var node2 = new PpReadFilesNode
		{
			InFiles = (node1.OutFiles, "path")
		};

		var node3b = new PpRegexNode()
		{
			// InPattern = @"^\[(?<ts>.+?) (?<level>[A-z]{1,5})\] \[(?<cat>.+?)\] (?<msg>.*)$",
			InPattern = @"INFO +(?<m1>[A-z0-9]+) +(?<m2>[A-z0-9]+)",
			OutMatch = (node2.OutLines, "line"),
		};

		var node4 = new PpDuckNode()
		{
			In = node3b.OutMatch,
			Query =
				"""
				select distinct m1, m2 from t1 limit 260
				""",
		};

		var node5 = new PpFormatNode
		{
			In = node4.OutIncl,
			Formatter = rec => $"{rec.Fields["m1"].ValueAsString?.PadRight(50, '.')}{rec.Fields["m2"].ValueAsString?.PadRight(50, '.')}",
		};

		var node6 = new PpCSharpNode
		{
			In = node4.OutIncl,
			Script =
				"""
				Rec["m1a"] = Rec["m1"]?.ToString()?.ToUpperInvariant();
				""",
		};

		await node1.ExecuteAsync();
		await node2.ExecuteAsync();
		// await node3.ExecuteAsync();
		await node3b.ExecuteAsync();
		await node4.ExecuteAsync();
		await node5.ExecuteAsync();
		await node6.ExecuteAsync();

		var m1 = node3b.OutMatch.DataFrame();
		var m2 = node3b.OutNoMatch.DataFrame();
		var m3 = node5.Out.DataFrame();

		sw.Stop();

		var res = node5.Out.DataFrame();
		var res6 = node6.Out.DataFrame();

		var dbg = 2;
	}
}