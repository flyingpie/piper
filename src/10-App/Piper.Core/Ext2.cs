using Blazor.Diagrams;
using Blazor.Diagrams.Core.Models;
using Piper.Core.Nodes;

namespace Piper.Core;

public static class Ext2
{
	public static PpGraph GetGraph(this BlazorDiagram diagram)
	{
		var graph = new PpGraph()
		{
			Nodes = diagram.Nodes.Cast<PpNodeBase>().ToList(),
		};

		return graph;
	}

	public static void LoadGraph(this BlazorDiagram diagram, PpGraph graph)
	{
		diagram.Links.Clear();
		diagram.Nodes.Clear();

		graph.Nodes.ForEach(n => diagram.Nodes.Add(n));

		foreach (var n in diagram.Nodes.OfType<PpNodeBase>())
		{
			foreach (var p in n.Ports)
			{
				var t = p.GetNodeInput?.Invoke()?.Output;
				if (t == null)
				{
					continue;
				}

				foreach (var n2 in diagram.Nodes.OfType<PpNodeBase>())
				{
					foreach (var p2 in n2.Ports)
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
}