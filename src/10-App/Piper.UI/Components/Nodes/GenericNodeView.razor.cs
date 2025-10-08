using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;
using Piper.Core.Attributes;
using Piper.Core.Data;
using Piper.Core.Nodes;
using Piper.UI.Services;
using System.Reflection;

namespace Piper.UI.Components.Nodes;

public partial class GenericNodeView<TNode>
{
	[Parameter]
	public GenericNodeModel<TNode> Model { get; set; } = null!;

	protected override Task OnInitializedAsync()
	{
		SelectedThingyService.Instance.OnChanged(() => InvokeAsync(() => StateHasChanged()));

		return Task.CompletedTask;
	}

	protected override Task OnParametersSetAsync()
	{
		return base.OnInitializedAsync();
	}

	private void OnNodeMouseDown()
	{
	}
}

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

public interface INodeProperty
{
}

public class MyParam : INodeProperty
{
	private object? _value;

	public string Name { get; set; }

	public object? Value
	{
		get => _value;
		set
		{
			_value = value;
			OnSet?.Invoke(this);
		}
	}

	public Action<MyParam> OnSet { get; set; }

	public int ValueAsInt
	{
		get => _value as int? ?? 0;
		set
		{
			_value = value;
			OnSet?.Invoke(this);
		}
	}

	public string ValueAsString
	{
		get => _value as string ?? string.Empty;
		set
		{
			_value = value;
			OnSet?.Invoke(this);
		}
	}
}

public class MyPortModel : PortModel, INodeProperty
{
	public MyPortModel(NodeModel parent, PortAlignment alignment = PortAlignment.Bottom, Point? position = null, Size? size = null)
		: base(parent, alignment, position, size)
	{
	}

	public MyPortModel(string id, NodeModel parent, PortAlignment alignment = PortAlignment.Bottom, Point? position = null, Size? size = null)
		: base(id, parent, alignment, position, size)
	{
	}

	public string Name => PortAttribute.Name;

	public PpPortAttribute PortAttribute { get; set; }

	public Func<PpNodeInput>? GetNodeInput { get; set; }

	public Func<PpNodeOutput>? GetNodeOutput { get; set; }

	public override void Refresh()
	{
		base.Refresh();

		if (Links.FirstOrDefault()?.Source?.Model is not MyPortModel src)
		{
			return;
		}

		if (Links.FirstOrDefault()?.Target?.Model is not MyPortModel dst)
		{
			return;
		}

		var srcNodeOutput = src.GetNodeOutput?.Invoke();
		if (srcNodeOutput == null)
		{
			return;
		}

		var dstNodeInput = dst.GetNodeInput?.Invoke();
		if (dstNodeInput == null)
		{
			return;
		}

		dstNodeInput.Table = srcNodeOutput.Table;

		Console.WriteLine($"Connected {src} to {dst}");
	}
}