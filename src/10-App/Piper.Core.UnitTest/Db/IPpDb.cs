using System.Threading.Tasks;

namespace Piper.Core.UnitTest.Db;

public interface IPpDb
{
	Task LoadAsync(PpDataFrame frame);

	Task<PpDataFrame> QueryAsync(string sql);
}