using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;
using Piper.Core;
using Piper.Core.Nodes;
using Piper.UI.Services;
using System.Reflection;

namespace Piper.UI.Components.Nodes;

public partial class GenericNodeView<TNode>
{
	[Parameter]
	public GenericNodeModel<TNode> Model { get; set; } = null!;

	// [Inject]
	// public SelectedThingyService? SelectedThingy { get; set; }

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
		// SelectedThingy.SelectNode(Node);
	}

	// public List<>
}

public class GenericNodeModel<TNode> : NodeModel
	where TNode : IPpNode
{
	public GenericNodeModel(TNode node)
	{
		Node = Guard.Against.Null(node);

		var props = node
				.GetType()
				.GetProperties(BindingFlags.Instance | BindingFlags.Public)
			;

		foreach (var prop in props)
		{
			// Param
			var paramAttr = prop.GetCustomAttribute<PpParam>();
			if (paramAttr != null)
			{
				NodeProps.Add(new MyParam()
				{
					Name = prop.Name,
					Value = prop.GetValue(node) as string,
					OnSet = v =>
					{
						Console.WriteLine($"ON SET ({v})");
						prop.SetValue(node, v);
					},
				});
				continue;
			}

			// Input
			var inAttr = prop.GetCustomAttribute<PpInput>();
			if (inAttr != null)
			{
				var pp = (MyPortModel)AddPort(new MyPortModel(this, PortAlignment.Left));
				pp.SelectDataFrame = () => ((PpNodeInput)prop.GetValue(node)).DataFrame?.Invoke();
				pp.GetNodeInput = () => (PpNodeInput)prop.GetValue(node);
				NodeProps.Add(pp);
				continue;
			}

			// Output
			var outAttr = prop.GetCustomAttribute<PpOutput>();
			if (outAttr != null)
			{
				var pp = (MyPortModel)AddPort(new MyPortModel(this, PortAlignment.Right));
				pp.SelectDataFrame = () => ((PpNodeOutput)prop.GetValue(node)).DataFrame?.Invoke();
				pp.GetNodeOutput = () => (PpNodeOutput)prop.GetValue(node);
				NodeProps.Add(pp);
				continue;
			}
		}

		var dbg = 2;
	}

	public TNode Node { get; }

	public List<INodeProperty> NodeProps { get; set; } = [];

	public IEnumerable<MyParam> Params => NodeProps.OfType<MyParam>();

	public IEnumerable<MyPortModel> Ports => NodeProps.OfType<MyPortModel>();
}

public interface INodeProperty
{
}

public class MyParam : INodeProperty
{
	private string? _value;

	public string Name { get; set; }

	public string? Value
	{
		get => _value;
		set
		{
			_value = value;
			OnSet?.Invoke(value);
		}
	}

	public Action<string?> OnSet { get; set; }
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

	// public Func<PpDataFrame> Type { get; set; }
	public Func<PpDataFrame>? SelectDataFrame { get; set; }

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

		var nodeSrc = src.GetNodeOutput?.Invoke();
		if (nodeSrc == null)
		{
			return;
		}

		var nodeDst = dst.GetNodeInput?.Invoke();
		if (nodeDst == null)
		{
			return;
		}

		nodeDst.DataFrame = () => nodeSrc.DataFrame?.Invoke() ?? PpDataFrame.Empty;

		// Console.WriteLine("Refresh");
	}
}