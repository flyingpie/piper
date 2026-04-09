using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Piper.Core.Attributes;
using Piper.Core.Data;
using Piper.Core.Data.Modifiers;
using Piper.Core.Serialization;
using VerifyMSTest;
using static VerifyMSTest.Verifier;

namespace Piper.Core.UnitTest.Serialization;

[TestClass]
[UsesVerify]
public partial class PpNodeSerializerTest
{
	[TestInitialize]
	public void Setup()
	{
		PpId.Instance.Reset();
	}

	/// <summary>
	/// Empty node, should result in mostly empty JSON.
	/// </summary>
	[TestMethod]
	public async Task Serialize_Node_Empty()
	{
		// Arrange
		var g = new PpGraph()
		{
			Nodes =
			[
				new PpTestNode()
				{
					//
					Name = "My Node 1",
					Position = new(123, 321),
				},
			],
		};

		// Act + Assert
		await VerifyJson(PpNodeSerializer.SerializeGraphJson(g));
	}

	[TestMethod]
	public async Task Serialize_Node_Param()
	{
		// Arrange
		var g = new PpGraph()
		{
			Nodes =
			[
				new PpTestNode()
				{
					Name = "My Node 1",
					Param1 = "Param1 Value",
					//
					// InPort1 = {  }"/path/to/dir",
				},
			],
		};

		// Act + Assert
		await VerifyJson(PpNodeSerializer.SerializeGraphJson(g));
	}

	[TestMethod]
	public async Task Serialize_Node_Port_In_WithMod_WithoutParams()
	{
		// Arrange
		var n1 = new PpTestNode() { Name = "My Node 1", InPort1 = { } };

		var n2 = new PpTestNode()
		{
			Name = "My Node 1",
			InPort1 =
			{
				// Node = n1,
				// Modifiers = { new PpCasingModifier() }
			},
		};

		n2.InPort1.Modifiers.Add(new PpTestMod());

		var g = new PpGraph()
		{
			Nodes =
			[
				// n1,
				n2,
			],
		};

		// Act + Assert
		await VerifyJson(PpNodeSerializer.SerializeGraphJson(g));
	}

	[TestMethod]
	public async Task Serialize_Node_Port_In_WithMod_WithParams()
	{
		// Arrange
		var n1 = new PpTestNode() { Name = "My Node 1", InPort1 = { } };

		var n2 = new PpTestNode()
		{
			Name = "My Node 1",
			InPort1 =
			{
				// Node = n1,
				// Modifiers = { new PpCasingModifier() }
			},
		};

		n2.InPort1.Modifiers.Add(new PpTestMod() { ModParam1 = "ModParam1 Value" });

		var g = new PpGraph()
		{
			Nodes =
			[
				// n1,
				n2,
			],
		};

		// Act + Assert
		await VerifyJson(PpNodeSerializer.SerializeGraphJson(g));
	}

	[TestMethod]
	public async Task Serialize_Node_Connected()
	{
		// Arrange
		var n1 = new PpTestNode() { Name = "My Node 1", InPort1 = { } };

		var n2 = new PpTestNode()
		{
			Name = "My Node 2",
			InPort1 =
			{
				// Node = n1,
				// Modifiers = { new PpCasingModifier() }
			},
		};

		n2.InPort1.Output = n1.OutPort1;

		var g = new PpGraph() { Nodes = [n1, n2] };

		// Act + Assert
		await VerifyJson(PpNodeSerializer.SerializeGraphJson(g));
	}

	[TestMethod]
	public void Deserialize_Node_Empty()
	{
		var graph = PpNodeSerializer.DeserializeGraph(
			"""
			[
				{
					"id": "node0042:PpTestNode:Test Node 1",
					"pos": "1234,4321",
					"params": {
						"Param1": "Param1 Value"
					},
					"ports": {
						"InPort1": {},
						"OutPort1": {}
					}
				}
			]
			"""
		);

		Assert.IsNotNull(graph);
		Assert.HasCount(1, graph.Nodes);

		var node1 = graph.Nodes[0];
		Assert.AreEqual("Test Node 1", node1.Name);
		Assert.AreEqual("node0042", node1.NodeId);
		Assert.AreEqual(1234, node1.Position.X);
		Assert.AreEqual(4321, node1.Position.Y);
	}
}

public class PpTestNode : PpNode
{
	public PpTestNode()
	{
		InPort1 = new(this, nameof(InPort1));
		OutPort1 = new(this, nameof(OutPort1));
	}

	[PpParam("Param1")]
	public string? Param1 { get; set; }

	[PpPort(PpPortDirection.Out, "InPort1")]
	public PpNodeInput InPort1 { get; }

	[PpPort(PpPortDirection.Out, "OutPort1")]
	public PpNodeOutput OutPort1 { get; }

	public override bool SupportsProgress => true;

	protected override Task OnExecuteAsync() => Task.CompletedTask;
}

public class PpTestMod : PpModifier
{
	public override string Name { get; set; } = "TestMod1";

	[PpParam("ModParam1")]
	public string? ModParam1 { get; set; }

	public override Task ExecuteAsync(IPpTable source, CancellationToken ct = default) => Task.CompletedTask;
}
