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
	public async Task Node_Empty()
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
	public async Task Node_Param()
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
	public async Task Node_Port_In_WithMod_WithoutParams()
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
	public async Task Node_Port_In_WithMod_WithParams()
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
	public async Task Node_Connected()
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
	public void METHOD()
	{
		var e = new List<string>();

		// e.GetType().IsAssignableTo(typeof(IEnumerable))

		var x = 2;
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
