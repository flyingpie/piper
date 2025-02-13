using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Piper.Core.Model;
using Tomlet;
using Tomlyn;

namespace Piper.Core.UnitTest;

[TestClass]
public class Test1
{
	[TestMethod]
	public void METHOD()
	{
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

		// var toml = TomletMain.TomlStringFrom(g, new TomlSerializerOptions() { });
		// File.WriteAllText(path, toml);

		var toml = File.ReadAllText(path);
		var x = TomletMain.To<PiperGraph>(toml);

		// Toml.ToModel<>();

		var dbg = 2;
	}
}
