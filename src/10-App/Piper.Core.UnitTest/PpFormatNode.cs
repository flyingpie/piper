using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Piper.Core.UnitTest;

public class PpFormatNode : IPpNode
{
	private PpDataFrame _outLines = new();

	public string? Name { get; set; }

	public Func<PpRecord, string> Formatter { get; set; }

	public PpNodeInput In { get; set; } = new();

	public PpNodeOutput Out { get; } = new();

	public async Task ExecuteAsync()
	{
		_outLines.Records.Clear();

		foreach (var rec in In.DataFrame().Records)
		{
			var ff = new Dictionary<string, PpField>(rec.Fields,
				StringComparer.OrdinalIgnoreCase);
			ff["fmt"] = new(Formatter(rec));

			_outLines.Records.Add(new PpRecord()
			{
				Fields = ff,
			});
		}

		Out.DataFrame = () => _outLines;
	}
}