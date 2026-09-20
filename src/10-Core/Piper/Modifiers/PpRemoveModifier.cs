using Piper.Core.Attributes;
using Piper.Core.Data;
using Piper.Core.Data.Modifiers;

namespace Piper.Modifiers;

public class PpRemoveModifier : PpModifier
{
	public override string Name { get; set; } = "Remove";

	[PpParam("Source Field")]
	public string? SrcFieldName { get; set; }

	public override async Task ExecuteAsync(IPpTable source, CancellationToken ct = default)
	{
		Guard.Against.Null(source);

		var sql = $"""
			create or replace view {Table.Name} as
				select		* exclude({SrcFieldName})
				from		{source.Name}
			""";

		await PpDb.Instance.LowLevel.ExecuteNonQueryAsync(sql, ct);
		await Table.InitFromDbAsync(ct);
	}
}
