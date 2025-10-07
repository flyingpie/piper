namespace Piper.Core.Db;

public interface IPpDb
{
	Task<long> CountAsync(string query);

	Task CreateTableAsync(PpTable table);

	Task InsertDataAsync(string tableName, IEnumerable<PpRecord> records);

	IAsyncEnumerable<PpRecord> QueryAsync(string query);
}