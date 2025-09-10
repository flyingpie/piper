using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Piper.Core.UnitTest;

[TestClass]
public class ScriptingTest
{
	public class MyGlobals
	{
		public Dictionary<string, PpField> Rec { get; set; } = new(StringComparer.OrdinalIgnoreCase)
		{
			{ "col1", "Val 1" },
			{ "col2", 42 },
		};
	}

	[TestMethod]
	public async Task METHOD()
	{
		var codeToEval =
			"""
			int test = 0;
			var count = test + 15;
			count++;
			//return count;
			//return Rec;

			Rec["col4"] = 1234;

			return true;
			""";

		var options = ScriptOptions.Default;

		var scr = CSharpScript.Create(codeToEval, options, typeof(MyGlobals));
		var diags = scr.Compile();

		// var result = await CSharpScript.EvaluateAsync(codeToEval, options);
		var glbs = new MyGlobals()
		{
		};

		var result = await scr.RunAsync(glbs, ex =>
		{
			var dbg2 = 2;

			return false;
		});

		var dbg = 2;
	}
}