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

	[Inject]
	public SelectedThingyService? SelectedThingy { get; set; }

	protected override Task OnInitializedAsync()
	{
		SelectedThingy.OnChanged(() => InvokeAsync(() => StateHasChanged()));

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

		var attrs = node
				.GetType()
				.GetProperties(BindingFlags.Instance | BindingFlags.Public)
			;

		// Params
		foreach (var param in attrs)
		{
			var paramAttr = param.GetCustomAttribute<PpParam>();
			if (paramAttr != null)
			{
				Params.Add(new()
				{
					Name = param.Name,
					Value = null,
					OnSet = v =>
					{
						Console.WriteLine($"ON SET ({v})");
						param.SetValue(node, v);
					},
				});
				continue;
			}
		}

		// Ports
		foreach (var port in attrs)
		{
			var inAttr = port.GetCustomAttribute<PpInput>();
			if (inAttr != null)
			{
				var pp = (MyPortModel)AddPort(new MyPortModel(this, PortAlignment.Left));
				pp.SelectDataFrame = ((PpNodeInput)port.GetValue(node)).DataFrame;
				pp.GetNodeInput = () => (PpNodeInput)port.GetValue(node);
				Ports.Add(pp);
				continue;
			}

			var outAttr = port.GetCustomAttribute<PpOutput>();
			if (outAttr != null)
			{
				var pp = (MyPortModel)AddPort(new MyPortModel(this, PortAlignment.Right));
				pp.SelectDataFrame = ((PpNodeOutput)port.GetValue(node)).DataFrame;
				pp.GetNodeOutput = () => (PpNodeOutput)port.GetValue(node);
				Ports.Add(pp);
				continue;
			}
		}

		var dbg = 2;
	}

	public TNode Node { get; }

	public List<MyParam> Params { get; set; } = [];

	public List<MyPortModel> Ports { get; set; } = [];
}

public class MyParam
{
	private string? _value;

	public string Name { get; set; }

	public string Value
	{
		get => _value;
		set
		{
			_value = value;
			OnSet?.Invoke(value);
		}
	}

	public Action<string> OnSet { get; set; }
}

public class MyPortModel : PortModel
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

		var nodeIn = src.GetNodeOutput?.Invoke();
		if (nodeIn == null)
		{
			return;
		}

		var nodeOut = dst.GetNodeInput?.Invoke();
		if (nodeOut == null)
		{
			return;
		}



		Console.WriteLine("Refresh");
	}
}