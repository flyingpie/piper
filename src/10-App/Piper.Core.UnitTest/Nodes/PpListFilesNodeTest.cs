using System.Linq;
using System.Threading.Tasks;
using Piper.Core.Nodes;

namespace Piper.Core.UnitTest.Nodes;

[TestClass]
public class PpListFilesNodeTest
{
	[TestMethod]
	public async Task METHOD()
	{
		var node = new PpListFilesNode()
		{
			//
			InPath = "/home/marco/Downloads",
			InPattern = "*.txt",
		};

		await node.ExecuteAsync();

		var res = node.OutFiles.Table();
		var x = await res.QueryAllAsync().Take(200).ToListAsync();

		var dbg = 2;
	}
}
