using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Piper.Core.UnitTest;

[TestClass]
public class MyGraphTest
{
	[TestMethod]
	public async Task METHOD()
	{
		var node1 = new PpListFilesNode
		{
			Name = "Node Name",
			InPath = "/home/marco/Downloads",
			InPattern = "logs-*.txt",
		};

		var node2 = new PpCatFilesNode
		{
			InFiles = (node1.OutFiles, "path")
		};

		// var node3 = new PpFormatNode
		// {
		// 	In = node2.OutLines,
		// 	Formatter = rec => $"{rec.Fields["ext"]}",
		// };

		var node3b = new PpRegexNode()
		{
			InPattern = @"^\[(?<ts>.+?) (?<level>[A-z]{1,5})\] \[(?<cat>.+?)\] (?<msg>.*)$",
			OutMatch = (node2.OutLines, "line"),
		};

		var node4 = new PpDuckNode()
		{
			In = node3b.OutMatch,
			Query =
				"""
				select distinct path from t1 limit 260
				""",
		};

		await node1.ExecuteAsync();
		await node2.ExecuteAsync();
		// await node3.ExecuteAsync();
		await node3b.ExecuteAsync();
		await node4.ExecuteAsync();

		var dbg = 2;
	}
}