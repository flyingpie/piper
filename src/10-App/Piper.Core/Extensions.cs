using Blazor.Diagrams;
using Blazor.Diagrams.Core.Models;
using Piper.Core.Attributes;
using Piper.Core.Data;
using System.Reflection;

namespace Piper.Core;

public static class Extensions
{
	public static PpGraph GetGraph(this BlazorDiagram diagram)
	{
		var graph = new PpGraph()
		{
			Nodes = diagram.Nodes.Cast<PpNode>().ToList(),
		};

		return graph;
	}

	public static void LoadGraph(this BlazorDiagram diagram, PpGraph graph)
	{
		diagram.Links.Clear();
		diagram.Nodes.Clear();

		graph.Nodes.ForEach(n => diagram.Nodes.Add(n));

		foreach (var n in diagram.Nodes.OfType<PpNode>())
		{
			foreach (var p in n.NodePorts)
			{
				var t = p.GetNodeInput?.Invoke()?.Output;
				if (t == null)
				{
					continue;
				}

				foreach (var n2 in diagram.Nodes.OfType<PpNode>())
				{
					foreach (var p2 in n2.NodePorts)
					{
						var t2 = p2.GetNodeOutput?.Invoke();
						if (t2 == null)
						{
							continue;
						}

						if (t == t2)
						{
							diagram.Links.Add(new LinkModel(p2, p));
						}
					}
				}
			}
		}
	}

	public static IReadOnlyCollection<IPpNodeProperty> GetNodeProps(this PpNode node)
		=> node.GetNodePropsInternal().ToList();

	private static IEnumerable<IPpNodeProperty> GetNodePropsInternal(this PpNode node)
	{
		var props = node
			.GetType()
			.GetProperties(BindingFlags.Instance | BindingFlags.Public);

		foreach (var prop in props)
		{
			// Param
			var paramAttr = prop.GetCustomAttribute<PpParamAttribute>();
			if (paramAttr != null)
			{
				yield return new PpNodeParam()
				{
					Name = prop.Name,
					// Value = prop.GetValue(node),
					Getter = () => prop.GetValue(node),
					Setter = v => prop.SetValue(node, v.Value),
				};
				continue;
			}

			// Port
			var inAttr = prop.GetCustomAttribute<PpPortAttribute>();
			if (inAttr != null)
			{
				if (prop.PropertyType == typeof(PpNodeInput))
				{
					var pp = new PpNodePort(node, PortAlignment.Left);
					pp.PortAttribute = inAttr;
					pp.GetNodeInput = () => (PpNodeInput)prop.GetValue(node)!;
					node.AddPort(pp);
					yield return pp;
				}
				else if (prop.PropertyType == typeof(PpNodeOutput))
				{
					var pp = new PpNodePort(node, PortAlignment.Right);
					pp.PortAttribute = inAttr;
					pp.GetNodeOutput = () => (PpNodeOutput)prop.GetValue(node)!;
					node.AddPort(pp);
					yield return pp;
				}
				else
				{
					Console.WriteLine($"No node port type found port '{node.Name}.{prop.Name}'");
				}

				// if (prop.GetValue(node) is PpNodeInput nodeInput)
				// {
				// }
				//
				// if (prop.GetValue(node) is PpNodeOutput nodeOutput)
				// {
				// }
			}
		}
	}
}