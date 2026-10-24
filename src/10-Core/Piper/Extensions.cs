using System.Reflection;
using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Models;
using Microsoft.Extensions.Logging;
using Piper.Core;
using Piper.Core.Modifiers;
using Piper.Core.Nodes;
using Piper.Utils;

namespace Piper;

public static class Extensions
{
	extension(PpPort port)
	{
		public long InCount => port.GetNodeInput?.Invoke()?.Output?.Table?.Count ?? 0;

		public long OutCount => port.GetNodeOutput?.Invoke()?.Table?.Count ?? 0;
	}

	public static PpGraph GetGraph(this Diagram diagram)
	{
		var graph = new PpGraph() { Nodes = diagram.Nodes.Cast<PpNode>().ToList() };

		return graph;
	}

	public static Guid Guid7() => Guid.CreateVersion7();

	public static void LoadGraph(this Diagram diagram, PpGraph graph)
	{
		var log = Log.For(typeof(Extensions));
		log.LogInformation("Loading graph '{Graph}'", graph);

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
							// var linkModel = new LinkModel(p2, p);

							var linkModel = (LinkModel)diagram.Options.Links.Factory(diagram, p2, new SinglePortAnchor(p));
							diagram.Links.Add(linkModel);
							// diagram.Controls.AddFor(linkModel, ControlsType.OnHover).Add(new RemoveControl(new LinkPathPositionProvider(.5)));
						}
					}
				}
			}
		}
	}

	public static IReadOnlyCollection<IPpProperty> GetModifierProps(this PpModifier mod) => mod.GetModifierPropsInternal().ToList();

	private static IEnumerable<IPpProperty> GetModifierPropsInternal(this PpModifier mod)
	{
		var props = mod.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

		foreach (var prop in props)
		{
			// Param
			var paramAttr = prop.GetCustomAttribute<PpParamAttribute>();
			if (paramAttr != null)
			{
				yield return new PpParam()
				{
					Name = prop.Name,
					// Name = paramAttr.Name,
					Hint = paramAttr.Hint,
					// Value = prop.GetValue(node),
					Type = prop.PropertyType,
					Getter = () => prop.GetValue(mod),
					Setter = v => prop.SetValue(mod, v),
				};
			}
		}
	}

	public static IReadOnlyCollection<IPpProperty> GetNodeProps(this PpNode node) => node.GetNodePropsInternal().ToList();

	private static IEnumerable<IPpProperty> GetNodePropsInternal(this PpNode node)
	{
		var props = node.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

		foreach (var prop in props)
		{
			// Param
			var paramAttr = prop.GetCustomAttribute<PpParamAttribute>();
			if (paramAttr != null)
			{
				yield return new PpParam()
				{
					Name = prop.Name,
					// Name = paramAttr.Name,
					Hint = paramAttr.Hint,
					// Value = prop.GetValue(node),
					Type = prop.PropertyType,
					Getter = () => prop.GetValue(node),
					Setter = v => prop.SetValue(node, v),
				};
				continue;
			}

			// Port
			var inAttr = prop.GetCustomAttribute<PpPortAttribute>();
			if (inAttr != null)
			{
				if (prop.PropertyType == typeof(PpNodeInput))
				{
					var pp = new PpPort(prop.Name, node, PortAlignment.Left)
					{
						//
						GetNodeInput = () => (PpNodeInput)prop.GetValue(node)!,
						Size = new(15, 15),
					};
					node.AddPort(pp);
					yield return pp;
				}
				else if (prop.PropertyType == typeof(PpNodeOutput))
				{
					var pp = new PpPort(prop.Name, node, PortAlignment.Right)
					{
						//
						GetNodeOutput = () => (PpNodeOutput)prop.GetValue(node)!,
						Size = new(15, 15),
					};
					node.AddPort(pp);
					yield return pp;
				}
				else
				{
					Console.WriteLine($"No node port type found port '{node.Name}.{prop.Name}'");
				}
			}
		}
	}
}
