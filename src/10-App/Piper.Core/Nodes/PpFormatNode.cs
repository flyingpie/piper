namespace Piper.Core.Nodes;

public class PpFormatNode : PpNodeBase
{
	private PpTable _outLines = new();

	public Func<PpRecord, string> Formatter { get; set; }

	public bool IsExecuting { get; }

	public PpNodeInput In { get; set; } = new();

	public PpNodeOutput Out { get; } = new();

	protected override async Task OnExecuteAsync()
	{
		await _outLines.ClearAsync();

		await foreach (var rec in In.Table().QueryAllAsync())
		{
			var ff = new Dictionary<string, PpField>(rec.Fields,
				StringComparer.OrdinalIgnoreCase);
			ff["fmt"] = new(Formatter(rec));

			await _outLines.AddAsync(new PpRecord()
			{
				Fields = ff,
			});
		}
	}
}