using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Piper.Core.Data;
using Piper.Core.Data.Modifiers;
using VerifyMSTest;
using static Piper.Core.Data.PpDataType;

namespace Piper.Core.UnitTest.Modifiers;

[TestClass]
[UsesVerify]
public partial class PpRazorModifierTest
{
	/// <summary>
	/// Successful execution, with various types.
	/// </summary>
	// csharpier-ignore-start
	[TestMethod]
	[DataRow(PpDataType.PpBool,			true,				"The value of 'col1' is 'True'")]
	[DataRow(PpDataType.PpBool,			false,				"The value of 'col1' is 'False'")]
	[DataRow(PpDataType.PpInt32,		int.MaxValue,		"The value of 'col1' is '2147483647'")]
	[DataRow(PpDataType.PpInt64,		long.MaxValue,		"The value of 'col1' is '9223372036854775807'")]
	[DataRow(PpDataType.PpString,		"My String",		"The value of 'col1' is 'My String'")]
	// csharpier-ignore-end
	public async Task Test(PpDataType type, object val, string expected)
	{
		// Arrange
		// var src = new PpTable(name: PpId.Instance.NextTable(), columns: [new(type, "col1")]);
		// await src.ClearAsync();
		// await src.AddRangeAsync([new(new() { { "col1", new PpField(type, val) } })]);
		// await src.AddAsync(
		// 	//
		// 	[("col1", type, val)]
		// );

		PpField f1 = ("name", "value");
		PpField f2 = ("name", true);
		PpField[] f = [("f1", true), ("f2", "a string")];

		PpRecord r1 = [new PpField(PpBool, true), new PpField(PpBool, true)];
		PpRecord r2 = [("f1", true), ("f2", "a string")];
		PpRecord r3 = [("f1", PpBool, "belH"), ("f2", "a string")];
		PpRecord r4 = [("f1", PpBool, "belH")];

		// var src3 = await new PpTable()
		// 	.WithColumns(
		// 		(PpBool, "the-bool"),
		// 		(PpString, "the-string")
		// 	)
		// 	.WithRecords(
		// 		[("the-bool", true), ("the-string", "The String")],
		// 		[("the-bool", true), ("the-string", "The String")]
		// 	)
		// 	.RecreateAsync();

		var src5 = await new PpTable()
			.AddAsync(
				// [("the-bool", true), ("the-string", "The String")],
				// [("the-bool", false), ("the-string", "Another Text")]
				// [("col1", true), ("col2", "The String")],
				[("col1", type, val)]
			);

		var list = await src5.QueryAllAsync().ToListAsync();

		// var src4 = await PpTable
		// 	.CreateAsync(
		// 		src5,
		// 		[("the-bool", true), ("the-string", "The String")],
		// 		[("the-bool", true), ("the-string", "The String")]
		// 	);

		// Act
		var mod = new PpRazorModifier()
		{
			//
			DstFieldName = "rzr_dst",
			Template = "The value of 'col1' is '@Rec.col1'",
			// Template = """
			// 	@if (Rec.col1) {
			// 		"S"
			// 	} else {
			// 		"Sup"
			// 	}
			// 	""",
		};

		await mod.ExecuteAsync(src5);
		await mod.Table.DoneAsync();

		var res = await mod.Table.QueryAllAsync().ToListAsync();

		Assert.HasCount(1, res);
		// Assert.AreEqual("The value of 'col1' is 'True'", res[0]["rzr_dst"]!.Value);
		Assert.AreEqual(expected, res[0]["rzr_dst"]!.Value);
		// await Verifier.Verify(res[0]);

		var dbg = 2;

		// var src2 = new PpTable();

		// await src2
		// 	.ClearColumns()
		// 	.WithColumns(
		// 		(PpBool, "the-bool"),
		// 		(PpString, "the-string")
		// 	)
		// 	.RecreateAsync();
		//
		// src2
		// 	.WithRecords(
		// 		[("the-bool", true), ("the-string", "The String")],
		// 		[("the-bool", true), ("the-string", "The String")]
		// 	);

		// [(PpBool, "the-bool"), (PpString, "the-string"), (PpInt32, 42)]
		// [(PpBool, "the-bool"), (PpString, "the-string"), (PpInt32, 42)]


		// var res = await mod.Table.QueryAllAsync().ToListAsync();
		//
		// // Assert
		// Assert.HasCount(1, res);
		// var rec = res[0];
		// Assert.AreEqual(expected, rec.Fields["rzr_dst"].Value);
	}

	/// <summary>
	/// Successful execution, with various types.
	/// </summary>
	[TestMethod]
	public async Task Test2()
	{
		// Arrange
		var src5 = await new PpTable()
			.AddAsync(
				// [("the-bool", true), ("the-string", "The String")],
				// [("the-bool", false), ("the-string", "Another Text")]
				// [("col1", true), ("col2", "The String")],
				[("col1", true)]
			);

		var list = await src5.QueryAllAsync().ToListAsync();

		// Act
		var mod = new PpRazorModifier()
		{
			//
			DstFieldName = "rzr_dst",
			Template = "The value of 'col1' is '@Rec.col1'",
		};

		await mod.ExecuteAsync(src5);
		await mod.Table.DoneAsync();

		var res = await mod.Table.QueryAllAsync().ToListAsync();

		Assert.HasCount(1, res);
		Assert.AreEqual("", res[0]["rzr_dst"]!.Value);
	}

	/// <summary>
	/// Razor template COMPILATION fails.
	/// </summary>
	[TestMethod]
	public async Task RazorCompilationError()
	{
		// Arrange
		// var src = new PpTable(name: $"ut_{PpId.Instance.NextTable()}", columns: [new PpColumn(PpDataType.PpBool, "col1")]);
		// await src.ClearAsync();
		// await src.AddRangeAsync([new PpRecord(new() { { "col1", true } })]);
		var src = await new PpTable()
			.AddAsync(
				// [("the-bool", true), ("the-string", "The String")],
				// [("the-bool", false), ("the-string", "Another Text")]
				// [("col1", true), ("col2", "The String")],
				[("col1", true)]
			);

		var mod = new PpRazorModifier() { DstFieldName = "rzr_dst", Template = "The result '@Rec.col1'" };

		mod.Template = "@NonExistentProperty";

		// Act
		await mod.ExecuteAsync(src);
		await mod.Table.DoneAsync();

		var res = await mod.Table.QueryAllAsync().ToListAsync();

		// Assert
		Assert.HasCount(0, res);

		mod.Logs.AssertLog(
			LogLevel.Error,
			"Error compiling Razor template: Unable to compile template:",
			"error CS0103: The name 'NonExistentProperty' does not exist in the current context"
		);
	}

	// /// <summary>
	// /// Razor template EXECUTION fails.
	// /// </summary>
	// [TestMethod]
	// public async Task RazorExecutionError()
	// {
	// 	// Arrange
	// 	var src = new PpTable(name: $"ut_{PpId.Instance.NextTable()}", columns: [new PpColumn(PpDataType.PpBool, "col1")]);
	// 	await src.ClearAsync();
	// 	await src.AddRangeAsync([new PpRecord(new() { { "col1", true } })]);
	//
	// 	var mod = new PpRazorModifier() { DstFieldName = "rzr_dst", Template = "The result '@Rec.col1'" };
	//
	// 	mod.Template = "@Rec.NonExistentProperty";
	//
	// 	// Act
	// 	await mod.ExecuteAsync(src);
	// 	await mod.Table.DoneAsync();
	//
	// 	var res = await mod.Table.QueryAllAsync().ToListAsync();
	//
	// 	// Assert
	// 	Assert.HasCount(1, res);
	// 	var rec = res[0];
	// 	Assert.AreEqual("Error running Razor template: 'System.Dynamic.ExpandoObject' does not contain a definition for 'NonExistentProperty'", rec.Fields["rzr_dst"].Value);
	// }
}
