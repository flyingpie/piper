namespace Piper.Core.Data;

public static class PpTableExtensions
{
	public static async Task InitAsync(this IPpTable table, PpRecord record, bool createTable = true)
	{
		Guard.Against.Null(table);

		await table.InitAsync(record.Select(rec => rec.AsColumn()), createTable);
	}
}
