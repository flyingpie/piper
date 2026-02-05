using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Piper.Core.Data;
using Piper.Core.Db;
using Piper.Core.Nodes;
using Piper.Core.Utils;

namespace Piper.Core.UnitTest;

[TestClass]
public class ScriptingTest
{
	public class MyGlobals
	{
		public Dictionary<string, PpField> Rec { get; set; } =
			new(StringComparer.OrdinalIgnoreCase) { { "col1", "Val 1" }, { "col2", 42 } };
	}

	[TestMethod]
	public async Task METHOD()
	{
		var codeToEval = """
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
		var glbs = new MyGlobals() { };

		var result = await scr.RunAsync(
			glbs,
			ex =>
			{
				var dbg2 = 2;

				return false;
			}
		);

		var dbg = 2;
	}

	[TestMethod]
	public async Task Test2()
	{
		var table = new PpTable(
			"my_table_1",
			[
				new("prop0_bool", PpDataType.PpBool),
				new("prop0_datetime", PpDataType.PpDateTime),
				new("prop0_float", PpDataType.PpFloat),
				new("prop0_guid", PpDataType.PpGuid),
				new("prop0_int", PpDataType.PpInt32),
				new("prop0_string", PpDataType.PpString),
			]
		);

		// Create Table
		await PpDb.Instance.CreateTableAsync(table);

		// Insert
		var data = new PpRecord[]
		{
			new()
			{
				Fields =
				{
					{
						"prop0_bool",
						new() { Value = true }
					},
					{
						"prop0_datetime",
						new() { Value = new DateTime(2024, 12, 31, 23, 45, 59) }
					},
					{
						"prop0_float",
						new() { Value = 1234.5F }
					},
					{
						"prop0_guid",
						new() { Value = Guid.AllBitsSet }
					},
					{
						"prop0_int",
						new() { Value = 1234 }
					},
					{
						"prop0_string",
						new() { Value = "my-string-yo" }
					},
				},
			},
			new()
			{
				Fields =
				{
					{
						"prop0_bool",
						new() { Value = false }
					},
					{
						"prop0_datetime",
						new() { Value = new DateTime(2099, 12, 31, 23, 45, 59) }
					},
					{
						"prop0_float",
						new() { Value = 5432.1F }
					},
					{
						"prop0_guid",
						new() { Value = Guid.Empty }
					},
					{
						"prop0_int",
						new() { Value = 4321 }
					},
					{
						"prop0_string",
						new() { Value = "another-string" }
					},
				},
			},
			new()
			{
				Fields =
				{
					{
						"prop0_bool",
						new() { Value = null }
					},
					{
						"prop0_datetime",
						new() { Value = null }
					},
					{
						"prop0_float",
						new() { Value = null }
					},
					{
						"prop0_guid",
						new() { Value = null }
					},
					{
						"prop0_int",
						new() { Value = null }
					},
					{
						"prop0_string",
						new() { Value = null }
					},
				},
			},
		};

		await PpDb.Instance.InsertDataAsync(table, data);

		// Query
		var res = await PpDb.Instance.QueryAsync(table, "select * from $table").ToListAsync();

		// Meta
		// var table2 = new PpTable("my_table_1");
		// await PpDb.Instance.V_InitTableAsync(table2);

		var json = PpJson.SerializeToString(res);

		var dbg = 2;
	}

	[TestMethod]
	public async Task PpListFilesNodeTest()
	{
		var node = new PpListFilesNode() { InPath = "/home/marco/Downloads", InPattern = "*.txt" };

		await node.ExecuteAsync();

		var outFilesTable = await node.OutFiles.Table().QueryAllAsync().ToListAsync();

		var dbg = 2;
	}

	[TestMethod]
	public async Task PMapAsync()
	{
		var pid = Process.GetProcessesByName("wtq")?.FirstOrDefault()?.Id ?? throw new InvalidOperationException();
		var smap = await GetSMapAsync(pid);

		var dbg = 2;
	}

	public static async Task<List<PpRecord>> GetSMapAsync(int pid)
	{
		var smapPath = $"/proc/{pid}/smaps";

		if (!File.Exists(smapPath))
		{
			return [];
		}

		var res = new List<PpRecord>();

		var lines = await File.ReadAllLinesAsync(smapPath);

		var headerRegex = new Regex(
			@"^(?<addr_from>[0-9a-f]+)-(?<addr_to>[0-9a-f]+) (?<perm>[^ ]+) (?<offset>[0-9a-f]+) (?<device>[0-9a-f]{2}:[0-9a-f]{2}) (?<inode>[0-9]+) +(?<module>.+)$"
		);
		var propRegex = new Regex(@"^(?<name>.+): +(?<val>.+)$");

		PpRecord? rec = null;

		foreach (var line in lines)
		{
			var match = headerRegex.Match(line);
			if (match.Success)
			{
				rec = new();
				foreach (Group grp in match.Groups)
				{
					if (grp.Index == 0)
					{
						continue;
					}

					rec.Fields[grp.Name] = grp.Value;
				}
				res.Add(rec);
				continue;
			}

			if (rec == null)
			{
				continue;
			}

			var match2 = propRegex.Match(line);
			if (match2.Success)
			{
				rec.Fields[match2.Groups["name"].Value] = match2.Groups["val"].Value;
			}
		}

		return res;
	}

	// private List<(string Name, int Length)> GetColumns(string header)
	// {
	// 	// string? col = null;
	//
	// 	var res = new List<string>();
	// 	var col = new StringBuilder();
	// 	var prev = ' ';
	//
	// 	for (var i = 0; i < header.Length; i++)
	// 	{
	// 		var c = header[i];
	// 		if (prev != ' ' && c == ' ')
	// 		{
	// 			res.Add(col.ToString());
	// 			col.Clear();
	// 		}
	// 		prev = c;
	//
	// 		col.Append(c);
	// 	}
	//
	// 	if (col.Length > 0)
	// 	{
	// 		res.Add(col.ToString());
	// 	}
	//
	// 	return res.Select(c => (c.Trim(), c.Length)).ToList();
	// }
}
