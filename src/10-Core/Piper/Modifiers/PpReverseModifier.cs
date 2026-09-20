namespace Piper.Modifiers;

public class PpReverseModifier : PpModifier
{
	public override string Name { get; set; } = "Reverse";

	public string? SrcFieldName { get; set; } = "path";

	public string? DstFieldName { get; set; } = "path_upper";

	public override async Task ExecuteAsync(IPpTable source, CancellationToken ct = default)
	{
		var sql = $"""
			create or replace view {Table.Name} as
				select		*
				,			reverse({SrcFieldName}) as {DstFieldName}
				from		{source.Name}
			""";

		await PpDb.Instance.LowLevel.ExecuteNonQueryAsync(sql, ct);
		await Table.InitFromDbAsync(ct);
	}
}
