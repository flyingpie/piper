namespace Piper.Modifiers;

public class PpCasingModifier : PpModifier
{
	public override string Name { get; set; } = "Casing";

	[PpParam("Source Field")]
	public string SrcFieldName { get; set; } = "path";

	[PpParam("Destination Field")]
	public string DstFieldName { get; set; } = "path_upper";

	public override async Task ExecuteAsync(IPpTable source, CancellationToken ct = default)
	{
		Guard.Against.Null(source);

		var sql = $"""
			create or replace view {Table.Name} as
				select		*
				,			upper({SrcFieldName}) as {DstFieldName}
				from		{source.Name}
			""";

		await PpDb.Instance.LowLevel.ExecuteNonQueryAsync(sql, ct);
		await Table.InitFromDbAsync(ct);
	}
}
