using System.Linq;
using System.Threading.Tasks;
using Piper.Core.Data;
using Piper.Core.Data.Modifiers;

namespace Piper.Core.UnitTest.Modifiers;

[TestClass]
public class PpCasingModifierTest
{
	[TestMethod]
	public async Task Upper_Full()
	{
		// Arrange
		var src = await new PpTable().AddAsync([("col1", "casing")]);

		var list = await src.QueryAllAsync().ToListAsync();

		// Act
		var mod = new PpCasingModifier()
		{
			//
			SrcFieldName = "col1",
			DstFieldName = "col2",
		};

		// var xx = await PpDb.Instance.QueryAsync(src, "select * from $table").ToListAsync();

		await mod.ExecuteAsync(src);
		await mod.Table.DoneAsync();

		var res = await mod.Table.QueryAllAsync().ToListAsync();

		Assert.HasCount(1, res);
		Assert.AreEqual("CASING", res[0]["col2"]!.Value);
	}

	[TestMethod]
	public async Task SrcFieldMissing()
	{
		// Arrange
		var src = await new PpTable().AddAsync([("col1", "casing")]);

		var list = await src.QueryAllAsync().ToListAsync();

		// Act
		var mod = new PpCasingModifier()
		{
			//
			SrcFieldName = "unknown_column",
			DstFieldName = "col2",
		};

		// var xx = await PpDb.Instance.QueryAsync(src, "select * from $table").ToListAsync();

		await mod.ExecuteAsync(src);
		await mod.Table.DoneAsync();

		var res = await mod.Table.QueryAllAsync().ToListAsync();

		Assert.HasCount(1, res);
		Assert.AreEqual("CASING", res[0]["col2"]!.Value);
	}
}
