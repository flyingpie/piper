using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Piper.Core.Data;
using Piper.Core.Db;
using Piper.Core.Nodes;
using Piper.Core.Utils;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

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


	[TestMethod]
	public async Task Test2()
	{
		var table = new PpTable(
			"my_table_1",
			[
				new("prop0_bool",			PpDataType.PpBool),
				new("prop0_datetime",		PpDataType.PpDateTime),
				new("prop0_float",			PpDataType.PpFloat),
				new("prop0_guid",			PpDataType.PpGuid),
				new("prop0_int",			PpDataType.PpInt),
				new("prop0_string",			PpDataType.PpString)
			]);

		// Create Table
		await PpDb.Instance.V_CreateTableAsync(table);

		// Insert
		var data = new PpRecord[]
		{
			new()
			{
				Fields =
				{
					{ "prop0_bool",			new() { Value = true } },
					{ "prop0_datetime",		new() { Value = new DateTime(2024, 12, 31, 23, 45, 59) } },
					{ "prop0_float",		new() { Value = 1234.5F } },
					{ "prop0_guid",			new() { Value = Guid.AllBitsSet } },
					{ "prop0_int",			new() { Value = 1234 } },
					{ "prop0_string",		new() { Value = "my-string-yo" } },
				},
			},
			new()
			{
				Fields =
				{
					{ "prop0_bool",			new() { Value = false } },
					{ "prop0_datetime",		new() { Value = new DateTime(2099, 12, 31, 23, 45, 59) } },
					{ "prop0_float",		new() { Value = 5432.1F } },
					{ "prop0_guid",			new() { Value = Guid.Empty } },
					{ "prop0_int",			new() { Value = 4321 } },
					{ "prop0_string",		new() { Value = "another-string" } },
				},
			},
			new()
			{
				Fields =
				{
					{ "prop0_bool",			new() { Value = null } },
					{ "prop0_datetime",		new() { Value = null } },
					{ "prop0_float",		new() { Value = null } },
					{ "prop0_guid",			new() { Value = null } },
					{ "prop0_int",			new() { Value = null } },
					{ "prop0_string",		new() { Value = null } },
				},
			},
		};

		await PpDb.Instance.V_InsertDataAsync(table, data);

		// Query
		var res = await PpDb.Instance.V_QueryAsync(table, "select * from $table").ToListAsync();

		// Meta
		var table2 = await PpDb.Instance.V_InitTableAsync("my_table_11");

		var json = PpJson.SerializeToString(res);

		var dbg = 2;
	}

	[TestMethod]
	public async Task Test1()
	{
		var node = new PpListFilesNode()
		{
			InPath = "/home/marco/Downloads",
			InPattern = "*.txt",
		};

		await node.ExecuteAsync();

		var outFilesTable = await node.OutFiles.Table().QueryAllAsync().ToListAsync();

		var dbg = 2;
	}
}