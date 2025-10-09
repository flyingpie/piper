using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Piper.Core.Attributes;
using Piper.Core.Data;

namespace Piper.Core;

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