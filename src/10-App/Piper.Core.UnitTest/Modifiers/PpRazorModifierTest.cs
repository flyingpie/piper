using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Piper.Core.Data;
using Piper.Core.Data.Modifiers;

namespace Piper.Core.UnitTest.Modifiers;

[TestClass]
public class PpRazorModifierTest
{
	/// <summary>
	/// Successful execution, with various types.
	/// </summary>
	// csharpier-ignore-start
	[TestMethod]
	[DataRow(PpDataType.PpBool,			true,				"The result 'True'")]
	[DataRow(PpDataType.PpBool,			false,				"The result 'False'")]
	[DataRow(PpDataType.PpInt32,		int.MaxValue,		"The result '2147483647'")]
	[DataRow(PpDataType.PpInt64,		long.MaxValue,		"The result '9223372036854775807'")]
	[DataRow(PpDataType.PpString,		"My String",		"The result 'My String'")]
	// csharpier-ignore-end
	public async Task Test(PpDataType type, object val, string expected)
	{
		// Arrange
		var src = new PpTable(name: PpId.Instance.NextTable(), columns: [new(type, "col1")]);
		await src.ClearAsync();
		await src.AddRangeAsync([new(new() { { "col1", new PpField(type, val) } })]);

		var src2 = PpTable
			.Create()
			.WithColumn(type, "col1")
			.WithRecord(
				[("col1","")]
			)
			.ClearAsync();

		// Act
		await mod.ExecuteAsync(src);
		await mod.Table.DoneAsync();

		var res = await mod.Table.QueryAllAsync().ToListAsync();

		// Assert
		Assert.HasCount(1, res);
		var rec = res[0];
		Assert.AreEqual(expected, rec.Fields["rzr_dst"].Value);
	}

	/// <summary>
	/// Razor template COMPILATION fails.
	/// </summary>
	[TestMethod]
	public async Task RazorCompilationError()
	{
		// Arrange
		var src = new PpTable(name: $"ut_{PpId.Instance.NextTable()}", columns: [new PpColumn(PpDataType.PpBool, "col1")]);
		await src.ClearAsync();
		await src.AddRangeAsync([new PpRecord(new() { { "col1", true } })]);

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

	/// <summary>
	/// Razor template EXECUTION fails.
	/// </summary>
	[TestMethod]
	public async Task RazorExecutionError()
	{
		// Arrange
		var src = new PpTable(name: $"ut_{PpId.Instance.NextTable()}", columns: [new PpColumn(PpDataType.PpBool, "col1")]);
		await src.ClearAsync();
		await src.AddRangeAsync([new PpRecord(new() { { "col1", true } })]);

		var mod = new PpRazorModifier() { DstFieldName = "rzr_dst", Template = "The result '@Rec.col1'" };

		mod.Template = "@Rec.NonExistentProperty";

		// Act
		await mod.ExecuteAsync(src);
		await mod.Table.DoneAsync();

		var res = await mod.Table.QueryAllAsync().ToListAsync();

		// Assert
		Assert.HasCount(1, res);
		var rec = res[0];
		Assert.AreEqual("Error running Razor template: 'System.Dynamic.ExpandoObject' does not contain a definition for 'NonExistentProperty'", rec.Fields["rzr_dst"].Value);
	}
}
