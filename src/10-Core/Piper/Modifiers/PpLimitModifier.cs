using Piper.Core.Attributes;
using Piper.Core.Data;
using Piper.Core.Data.Modifiers;

namespace Piper.Modifiers;

public class PpLimitModifier : PpModifier
{
	public override string Name { get; set; } = "Limit";

	[PpParam("Skip")]
	public int Skip { get; set; }

	[PpParam("Take")]
	public int Take { get; set; }

	public override async Task ExecuteAsync(IPpTable source, CancellationToken ct = default)
	{
		Guard.Against.Null(source);

		var sql = $"""
			create or replace view {Table.Name} as
				select		*
				from		{source.Name}
				offset		{Skip}
				limit		{Take}
			""";

		await PpDb.Instance.LowLevel.ExecuteNonQueryAsync(sql, ct);
		await Table.InitFromDbAsync(ct);
	}
}
