namespace Piper.Core.Db;

public interface IPpDb
{
	Task LoadAsync(PpDataFrame frame);

	Task<PpDataFrame> QueryAsync(string sql);
}