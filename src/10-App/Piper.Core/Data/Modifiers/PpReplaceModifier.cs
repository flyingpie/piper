using Piper.Core.Attributes;
using Piper.Core.Db;

namespace Piper.Core.Data.Modifiers;

public class PpReplaceModifier : PpModifier
{
	public override string Name { get; set; } = "Replace";

	[PpParam("Source Field")]
	public string SrcFieldName { get; set; }

	[PpParam("Destination Field")]
	public string DstFieldName { get; set; }

	public override async Task ExecuteAsync(IPpTable source, CancellationToken ct = default)
	{
		Guard.Against.Null(source);

		var sql = $"""
			create or replace view {Table.Name} as
				select		* replace({DstFieldName} as {SrcFieldName})
				from		{source.Name}
			""";

		await PpDb.Instance.LowLevel.ExecuteNonQueryAsync(sql, ct);
		await PpDb.Instance.FetchTableAsync(Table, ct);
	}
}
