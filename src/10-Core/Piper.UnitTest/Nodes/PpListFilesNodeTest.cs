using System.Linq;
using System.Threading.Tasks;
using Piper.Modifiers;
using Piper.Nodes;

namespace Piper.UnitTest.Nodes;

[TestClass]
public class PpListFilesNodeTest
{
	[TestMethod]
	public async Task METHOD()
	{
		var node = new PpListFilesNode()
		{
			//
			InPath = "/home/marco/workspace/flyingpie/hk_1",
			InPattern = "**/*",
		};

		node.OutFiles.Modifiers.Add(
			new PpSelectModifier()
			{
				//
				DstFieldName = "path",
				Expression = "file->>'path'",
			}
		);

		node.OutFiles.Modifiers.Add(
			new PpReadFilesModifier()
			{
				//
				SrcFieldName = "path",
				DstFieldName = "content",
			}
		);

		await node.ExecuteAsync();

		var res = node.OutFiles.Table;
		var x = await res.QueryAllAsync().Take(200).ToListAsync();

		var dbg = 2;
	}
}
