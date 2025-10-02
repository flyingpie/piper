using System.Text.RegularExpressions;

namespace Piper.Core.Nodes;

public class PpRegexNode : IPpNode
{
	private PpDataFrame _match = new();
	private PpDataFrame _noMatch = new();

	public string NodeType => "Regex";

	public string Name { get; set; }

	public bool IsExecuting { get; }

	public PpNodeInput InPattern { get; set; }

	public PpNodeInput OutMatch { get; set; } = new();

	public PpNodeOutput OutNoMatch { get; } = new();

	public async Task ExecuteAsync()
	{
		_match.Records.Clear();
		_noMatch.Records.Clear();

		var regex = new Regex(InPattern.Value, RegexOptions.Compiled);

		var resMatch = new List<PpRecord>();

		var resNoMatch = new List<PpRecord>();

		foreach (var rec in OutMatch.DataFrame().Records)
		{
			var f = rec.Fields.ToDictionary();

			var match = regex.Match(f[OutMatch.AttributeName].ValueAsString);
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

				resMatch.Add(rec2);
			}
			else
			{
				var rec2 = new PpRecord() { Fields = f, };
				resNoMatch.Add(rec2);
			}
		}

		_match = new() { Records = resMatch, };

		_noMatch = new() { Records = resNoMatch, };

		OutMatch.DataFrame = () => _match;
		OutNoMatch.DataFrame = () => _noMatch;
	}
}