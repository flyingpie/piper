namespace Piper.Core.Nodes;

public class PpRegexNode : PpNodeBase
{
	private PpTable _match = new();
	private PpTable _noMatch = new();

	public PpRegexNode()
	{
		In = new();

		OutMatch = new() { Table = () => _match, };
		OutNoMatch = new() { Table = () => _noMatch, };
	}

	public string NodeType => "Regex";

	public string Name { get; set; }

	public bool IsExecuting { get; }

	[PpParam("Pattern")]
	public string? InPattern { get; set; }

	[PpParam("Attribute")]
	public string? InAttribute { get; set; }

	[PpInput("Input")]
	public PpNodeInput In { get; set; }

	[PpOutput("Match")]
	public PpNodeOutput OutMatch { get; }

	[PpOutput("No Match")]
	public PpNodeOutput OutNoMatch { get; }

	protected override async Task OnExecuteAsync()
	{
		if (!In.IsConnected)
		{
			LogWarning($"Port '{In}' not connected, stopping");
			return;
		}

		if (string.IsNullOrWhiteSpace(InAttribute))
		{
			LogWarning("No attribute specified, stopping");
			return;
		}

		await _match.ClearAsync();
		await _noMatch.ClearAsync();

		var regex = new Regex(InPattern ?? string.Empty, RegexOptions.Compiled);

		await foreach (var rec in In.Table().QueryAllAsync())
		{
			var f = rec.Fields.ToDictionary();

			var match = regex.Match(f[InAttribute].ValueAsString);
			if (match.Success)
			{
				foreach (var grp in match.Groups.OfType<Group>())
				{
					if (!char.IsAsciiLetter(grp.Name[0]))
					{
						continue;
					}

					f[grp.Name] = new(grp.Value);
				}

				var rec2 = new PpRecord() { Fields = f, };
				await _match.AddAsync(rec2);
			}
			else
			{
				var rec2 = new PpRecord() { Fields = f, };
				await _noMatch.AddAsync(rec2);
			}
		}
	}
}