using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace Piper.Core.Nodes;

public class PpCSharpNode : IPpNode
{
	public class MyGlobals
	{
		public Dictionary<string, PpField> Rec { get; set; }
	}

	public string NodeType => "Script";

	public string Name { get; set; }

	private PpDataFrame _outLines = new();

	public string Script { get; set; }

	public bool IsExecuting { get; }

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

			var glbs = new MyGlobals() { Rec = ff, };

			var result = await scr
				.RunAsync(glbs, ex =>
				{
					Console.WriteLine($"EXCEPTION: {ex}");
					return false;
				})
				.ConfigureAwait(true);

			_outLines.Records.Add(new PpRecord() { Fields = glbs.Rec, });
		}

		Out.DataFrame = () => _outLines;
	}
}