using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Piper.Core.Nodes;
using Piper.Core.Serialization;
using VerifyMSTest;
using static VerifyMSTest.Verifier;

namespace Piper.Core.UnitTest.Serialization;

[TestClass]
[UsesVerify]
public partial class PpNodeSerializerTest
{
	[TestMethod]
	public async Task Node_Empty()
	{
		// Arrange
		var g = new PpGraph()
		{
			Nodes =
			[
				new PpReadCsvNode()
				{
					//
				},
			],
		};

		// Act
		var actual = PpNodeSerializer.SerializeGraphJson(g);

		var expected = """
			[
				{
					"id" : "rlP4HUqELNy8Y:PpReadCsvNode:Node",
					"pos" : "0,0",
					"ports" : {
						"InFiles" : {
							"mods" : [ ]
						},
						"OutRecords" : {
							"mods" : [ ]
						},
						"OutFailures" : {
							"mods" : [ ]
						}
					}
				}
			]
			""";
		var expectedJson = JsonNode.Parse(expected);

		// Assert
		await VerifyJson(actual);
	}
}
