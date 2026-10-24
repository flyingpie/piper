using System.Linq;
using System.Threading.Tasks;
using Piper.Core.Data;
using Piper.Core.Modifiers;

namespace Piper.UnitTest.Core.Modifiers;

[TestClass]
public class PpModifierStackTest
{
	private PpTable _baseTable = null!;

	private PpModifierStack _sut = null!;

	[TestInitialize]
	public void Setup()
	{
		_baseTable = new PpTable();

		_sut = new(() => _baseTable);
	}

	[TestMethod]
	public async Task Empty()
	{
		await _sut.ExecuteAsync();

		var recs = await _sut.Table.QueryAllAsync().ToListAsync();

		Assert.HasCount(0, recs);
	}
}
