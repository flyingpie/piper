using Blazor.Diagrams.Core.Models;
using Piper.Core.Attributes;
using Piper.Core.Data;
using Piper.Core.Nodes;
using System.Reflection;

namespace Piper.Core;

public class GenericNodeModel : NodeModel
{
	public List<INodeProperty> NodeProps { get; set; } = [];

	public IEnumerable<MyParam> Params => NodeProps.OfType<MyParam>();

	public IEnumerable<MyPortModel> Ports => NodeProps.OfType<MyPortModel>();
}

public class GenericNodeModel<TNode> : GenericNodeModel
	where TNode : IPpNode
{
	public GenericNodeModel(TNode node)
	{
		Node = Guard.Against.Null(node);

		var props = node
			.GetType()
			.GetProperties(BindingFlags.Instance | BindingFlags.Public);

		foreach (var prop in props)
		{
			// Param
			var paramAttr = prop.GetCustomAttribute<PpParamAttribute>();
			if (paramAttr != null)
			{
				NodeProps.Add(new MyParam()
				{
					Name = prop.Name,
					Value = prop.GetValue(node),
					OnSet = v =>
					{
						Console.WriteLine($"ON SET ({v})");
						prop.SetValue(node, v.Value);
					},
				});
				continue;
			}

			// Port
			var inAttr = prop.GetCustomAttribute<PpPortAttribute>();
			if (inAttr != null)
			{
				if (prop.GetValue(node) is PpNodeInput nodeInput)
				{
					var pp = (MyPortModel)AddPort(new MyPortModel(this, PortAlignment.Left));
					pp.PortAttribute = inAttr;
					pp.GetNodeInput = () => nodeInput;
					NodeProps.Add(pp);
				}

				if (prop.GetValue(node) is PpNodeOutput nodeOutput)
				{
					var pp = (MyPortModel)AddPort(new MyPortModel(this, PortAlignment.Right));
					pp.PortAttribute = inAttr;
					pp.GetNodeOutput = () => nodeOutput;
					NodeProps.Add(pp);
				}
			}
		}
	}

	public TNode Node { get; }
}