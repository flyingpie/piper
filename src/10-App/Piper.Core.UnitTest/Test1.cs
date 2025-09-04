using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Piper.Core.Model;
using Tomlet;
using Tomlet.Models;

namespace Piper.Core.UnitTest;

[TestClass]
public class Test1
{
	// [TestMethod]
	public void METHOD()
	{
		TomletMain.RegisterMapper<System.Drawing.Point>(
			clrPoint =>
			{
				// var t = new TomlTable();
				//
				// t.Entries["x"] = new TomlLong(12);
				// t.Entries["y"] = new TomlLong(34);
				//
				// return t;

				var a = new TomlArray();

				a.Add(clrPoint.X);
				a.Add(clrPoint.Y);

				return a;
			},
			tomlPoint =>
			{
				// if (tomlPoint is not TomlTable asTable)
				// {
				// 	// TODO: Warn
				// 	return Point.Empty;
				// }
				//
				// var x = asTable.GetInteger("x");
				// var y = asTable.GetInteger("y");
				//
				// return new System.Drawing.Point(x, y);

				if (tomlPoint is not TomlArray tomlArray)
				{
					return Point.Empty;
				}

				if (tomlArray.Count != 2)
				{
					return Point.Empty;
				}

				if (tomlArray[0] is not TomlLong tomlLongX)
				{
					return Point.Empty;
				}

				if (tomlArray[1] is not TomlLong tomlLongY)
				{
					return Point.Empty;
				}

				return new((int)tomlLongX.Value, (int)tomlLongY.Value);
			}
		);

		// TomletMain.RegisterMapper<PiperGraph>(
		// 	graph =>
		// 	{
		// 		var t = new TomlTable();
		//
		// 		// foreach (var node in graph.Nodes)
		// 		// {
		// 		// 	t.Put($"node.{node.Id}", TomletMain.ValueFrom(node));
		// 		// }
		//
		// 		var arr = new TomlArray();
		// 		foreach (var n in graph.Nodes)
		// 		{
		// 			arr.Add((TomlTable)TomletMain.ValueFrom(n));
		// 		}
		//
		// 		t.Put("node", arr);
		//
		// 		return t;
		// 	},
		// 	tomlGraph =>
		// 	{
		// 		return new();
		// 	}
		// );

		// TomletMain.RegisterMapper<PiperNodeModel>(
		// 	node =>
		// 	{
		// 		var t = new TomlTable();
		// 		t.ForceNoInline = true;
		//
		// 		// t.ShouldBeSerializedInline = false;
		// 		t.Put("type", node.NodeType);
		// 		t.Put("name", node.Name);
		// 		t.Put("id", node.Id);
		//
		// 		return t;
		// 	},
		// 	tomlNode =>
		// 	{
		// 		return new();
		// 	}
		// );

		// TomletMain.RegisterMapper<PiperPortModel>(
		// 	port =>
		// 	{
		// 		// var t = new TomlTable();
		// 		//
		// 		// t.Put("name", port.Name);
		//
		// 		var t = TomletMain.ValueFrom(port);
		//
		// 		return t;
		// 	},
		// 	toml =>
		// 	{
		// 		return new();
		// 	}
		// );

		TomletMain.RegisterMapper<Dictionary<string, PiperPortModel>>(
			dict =>
			{
				var t = new TomlTable();

				foreach (var (k, v) in dict)
				{
					t.Put(k, TomletMain.ValueFrom(v));
				}

				return t;
			},
			toml =>
			{
				return new();
			}
		);

		var g = new PiperGraph();

		// g.Nodes.Add(new PiperNodeModel() { });
		// g.Nodes.Add(new PiperNodeModel() { });

		// var toml = Toml.FromModel(
		// 	g,
		// 	new TomlModelOptions()
		// 	{
		// 		CreateInstance = (t, objKind) =>
		// 		{
		// 			return null;
		// 		},
		// 	}
		// );

		var path = "/home/marco/Downloads/piper2.toml";

		var toml1 = TomletMain.TomlStringFrom(g, new TomlSerializerOptions() { });

		File.WriteAllText(path, toml1);

		var toml2 = File.ReadAllText(path);
		var x = TomletMain.To<PiperGraph>(toml2);

		// Toml.ToModel<>();

		var dbg = 2;
	}
}
