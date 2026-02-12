using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.Scripting.Hosting;
using Piper.Core.Attributes;
using Piper.Core.Data;
using static Piper.Core.Data.PpPortDirection;

namespace Piper.Core.Nodes;

public class PpCSharpNode : PpNode
{
	public class MyGlobals
	{
		public Dictionary<string, PpField> Rec { get; set; } = new();
	}

	public PpCSharpNode()
	{
		InRecords = new(this, nameof(InRecords));
		OutRecords = new(this, nameof(OutRecords), new(PpTable.GetTableName(this, nameof(OutRecords))));
	}

	public override string Color => "#8a2828";

	public override string Icon => "fa-solid fa-code";

	public override string NodeType => "Script";

	public override bool SupportsProgress => false;

	[PpParam("Script", Hint = PpParamHint.Code)]
	public string Script { get; set; }

	[PpPort(Out, "InRecords")]
	public PpNodeInput InRecords { get; set; }

	[PpPort(Out, "OutRecords")]
	public PpNodeOutput OutRecords { get; }

	protected override async Task OnExecuteAsync()
	{
		var cols = InRecords.Output.Table.Columns.ToList();
		cols.Add(new("out1", PpDataType.PpString));
		OutRecords.Table.Columns.Clear();
		OutRecords.Table.Columns.AddRange(cols);
		await OutRecords.Table.ClearAsync();

		var options = ScriptOptions.Default;
		options = options.WithImports(
			"System"
		// "System.Data",
		// "System.Data.SqlClient",
		// "System.Windows.Forms",
		// "CustomHelper",
		// "CustomHelper.MainLibrary"
		);
		// options = options.AddReferences(Assembly.LoadFrom("CustomHelper.dll")); // our custom Roslyn DLL

		var scr = CSharpScript.Create(Script, options, typeof(MyGlobals), new InteractiveAssemblyLoader());
		var diags = scr.Compile();

		{
			var appender = await OutRecords.Table.CreateAppenderAsync();

			await foreach (var rec in InRecords.Output.Table.QueryAllAsync())
			{
				var ff = new Dictionary<string, PpField>(rec.Fields, StringComparer.OrdinalIgnoreCase);

				var glbs = new MyGlobals() { Rec = ff };

				var result = await scr.RunAsync(
						glbs,
						ex =>
						{
							Console.WriteLine($"EXCEPTION: {ex}");
							return false;
						}
					)
					.ConfigureAwait(true);

				appender.Add(new PpRecord() { Fields = glbs.Rec });
			}
		}

		await OutRecords.Table.DoneAsync();
	}
}
