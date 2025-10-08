using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Piper.Core.Data;

namespace Piper.Core.Nodes;

public class PpCSharpNode : PpNodeBase
{
	public class MyGlobals
	{
		public Dictionary<string, PpField> Rec { get; set; }
	}

	private PpTable _outLines = new("todo");

	public string Script { get; set; }

	public PpNodeInput In { get; set; } = new();

	public PpNodeOutput Out { get; } = new();

	protected override async Task OnExecuteAsync()
	{
		await _outLines.ClearAsync();

		var options = ScriptOptions.Default;

		var scr = CSharpScript.Create(Script, options, typeof(MyGlobals));
		var diags = scr.Compile();

		await foreach (var rec in In.Table().QueryAllAsync())
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

			await _outLines.AddAsync(new PpRecord() { Fields = glbs.Rec, });
		}
	}
}