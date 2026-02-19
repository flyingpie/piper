using Piper.Core.Attributes;
using Piper.Core.Db;

namespace Piper.Core.Data.Modifiers;

public class PpSelectModifier : PpModifier
{
	public override string Name { get; set; } = "Select";

	[PpParam("Expression")]
	public string? Expression { get; set; }

	[PpParam("Destination Field")]
	public string DstFieldName { get; set; }

	public override async Task ExecuteAsync(IPpTable source, CancellationToken ct = default)
	{
		Guard.Against.Null(source);

		var sql = $"""
			create or replace view {Table.Name} as
				select		*
				,			{Expression} as {DstFieldName}
				from		{source.Name}
			""";

		await PpDb.Instance.LowLevel.ExecuteNonQueryAsync(sql, ct);
		await PpDb.Instance.FetchTableAsync(Table, ct);
	}
}
