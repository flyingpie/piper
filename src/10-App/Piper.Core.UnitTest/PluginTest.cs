using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Piper.Core.Data;

namespace Piper.Core.UnitTest;

[TestClass]
public class PluginTest
{
	[TestMethod]
	public async Task Test1()
	{
		var path = "/home/marco/Downloads/scripts/build/GuidModifier.dll";

		var loader = new PluginLoadContext();
		var ass = loader.LoadFromAssemblyPath(path);

		var types = ass.GetTypes();
		var t1 = types.Where(t => t.GetCustomAttribute<DataContractAttribute>() != null).ToList();

		foreach (var t in t1)
		{
			var props = t.GetProperties(BindingFlags.Instance | BindingFlags.Public)
				.Where(p => p.GetCustomAttribute<BrowsableAttribute>() != null)
				.ToList();

			var methods = t.GetMethods(BindingFlags.Instance | BindingFlags.Public);

			foreach (var meth in methods)
			{
				var x4 = meth.ReturnType;
				var x_ = x4 == typeof(Task);
				if (meth.ReturnType != typeof(Task))
				{
					continue;
				}

				var x6 = meth.GetParameters();
				if (x6.Length != 1)
				{
					continue;
				}
				var p1 = x6[0];
				var y_ = p1.ParameterType == typeof(Dictionary<string, object?>);
				if (p1.ParameterType != typeof(Dictionary<string, object?>))
				{
					continue;
				}

				var p = new Dictionary<string, object?>();
				var obj = Activator.CreateInstance(t);
				props.First().SetValue(obj, "my_field_name_yo");
				var res = (Task)meth.Invoke(obj, [p]);
				await res;

				var xx = 2;
			}
		}

		var dbg = 2;
	}
}
