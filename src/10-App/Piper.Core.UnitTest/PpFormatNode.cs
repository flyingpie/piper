using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

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

public class PpCSharpNode : IPpNode
{
	public class MyGlobals
	{
		public Dictionary<string, PpField> Rec { get; set; }
	}

	private PpDataFrame _outLines = new();

	public string Script { get; set; }

	public PpNodeInput In { get; set; } = new();

	public PpNodeOutput Out { get; } = new();

	public async Task ExecuteAsync()
	{
		_outLines.Records.Clear();

		var options = ScriptOptions.Default;

		var scr = CSharpScript.Create(Script, options, typeof(MyGlobals));
		var diags = scr.Compile();

		foreach (var rec in In.DataFrame().Records)
		{
			var ff = new Dictionary<string, PpField>(rec.Fields, StringComparer.OrdinalIgnoreCase);
			// ff["fmt"] = new(Formatter(rec));

			var glbs = new MyGlobals()
			{
				Rec = ff,
			};

			var result = await scr.RunAsync(glbs, ex =>
			{
				Console.WriteLine($"EXCEPTION: {ex}");
				return false;
			});

			_outLines.Records.Add(new PpRecord()
			{
				Fields = ff,
			});
		}

		Out.DataFrame = () => _outLines;
	}
}