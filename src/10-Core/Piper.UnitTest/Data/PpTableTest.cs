using System;
using System.Linq;
using System.Threading.Tasks;
using Piper.Core.Data;
using static Piper.Core.Extensions;

namespace Piper.UnitTest.Data;

[TestClass]
public class PpTableTest
{
	private readonly Guid Guid1 = Guid7();
	private readonly Guid Guid2 = Guid7();

	private PpTable _table = null!;

	[TestInitialize]
	public async Task Setup()
	{
		_table = new PpTable();
	}

	[TestMethod]
	public async Task Add_Multiple()
	{
		// Act
		await using (var appender = await _table.CreateAppenderAsync())
		{
			var rec1 = new PpRecord([
				("col1", true), // Bool
				("col2", Guid1), // Guid
				("col3", 1234), // Int32
				("col4", "The First Record"), // String
			]);

			var rec2 = new PpRecord([
				("col1", false), // Bool
				("col2", Guid2), // Guid
				("col3", 4321), // Int32
				("col4", "The Second Record"), // String
			]);

			appender.AddAsync(rec1);
			appender.AddAsync(rec2);
		}

		// Assert
		var recs = await _table.QueryAllAsync().ToListAsync();

		Assert.HasCount(2, recs);

		recs[0].AssertField("col1", PpDataType.PpBool, true);
		recs[0].AssertField("col2", PpDataType.PpGuid, Guid1);
		recs[0].AssertField("col3", PpDataType.PpInt32, 1234);
		recs[0].AssertField("col4", PpDataType.PpString, "The First Record");

		recs[1].AssertField("col1", PpDataType.PpBool, false);
		recs[1].AssertField("col2", PpDataType.PpGuid, Guid2);
		recs[1].AssertField("col3", PpDataType.PpInt32, 4321);
		recs[1].AssertField("col4", PpDataType.PpString, "The Second Record");
	}

	[TestMethod]
	public async Task Add_None()
	{
		// Assert
		var recs = await _table.QueryAllAsync().ToListAsync();

		Assert.HasCount(0, recs);
	}
}

public static class AssertUtils
{
	public static void AssertField(this PpRecord record, string name, PpDataType type, object? value)
	{
		Assert.IsTrue(record.TryGetField(name, out var f));
		Assert.AreEqual(type, f.DataType);
		Assert.AreEqual(value, f.Value);
	}
}
