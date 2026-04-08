using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Piper.Core.Data;
using Piper.Core.Nodes;
using static Piper.Core.Data.PpDataType;

namespace Piper.Core.UnitTest.Nodes;

[TestClass]
public class PpRegexNodeTest
{
	[TestMethod]
	public async Task TestName()
	{
		// Arrange
		var stubNode = new PpStubNode();

		var outRecords = new PpNodeOutput(stubNode, "Out");
		outRecords.BaseTable.Columns =
		[
			new(PpString, "src"),
		];

		await outRecords.BaseTable.ClearAsync();

		{
			await using var appender = await outRecords.BaseTable.CreateAppenderAsync();
			appender.Add(new PpRecord()
			{
				Fields = 
				{
					{ "src", new(PpString, "Some text some more text and even more text") },
				},
			});
			appender.Add(new PpRecord()
			{
				Fields = 
				{
					{ "src", new(PpString, "Some text #aabbcc some more text") },
				},
			});
			appender.Add(new PpRecord()
			{
				Fields = 
				{
					{ "src", new(PpString, "Some text #aabbcc some more text #002244 and even more text") },
				},
			});
		}

		await outRecords.Table.DoneAsync();

		var node = new PpRegexNode()
		{
			InAttribute = "src",
			InPattern = @"#[A-Fa-f0-9]{6}\b",
			InRecords = { Output = outRecords, },
		};

		// Act
		await node.ExecuteAsync();
	
		// Assert
		var res = node.OutMatch.Table;
		var x = await res.QueryAllAsync().ToListAsync();

		var dbg = 2;
	}
}


// public class PpNodeInputMock(params PpRecord[] records) : PpNodeInput
// {
// }
