using System.Threading.Tasks;

namespace Piper.Core.UnitTest.Db;

public class PgPpDb : IPpDb
{
	public Task LoadAsync(PpDataFrame frame)
	{
		throw new System.NotImplementedException();
	}

	public Task<PpDataFrame> QueryAsync(string sql)
	{
		throw new System.NotImplementedException();
	}
}