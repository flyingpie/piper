using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Piper.Core.Attributes;
using Piper.Core.Data;

namespace Piper.Core;

public class PpNodePort : PortModel, IPpNodeProperty
{
	private Action<PpNodePort>? _onChange;

	public PpNodePort(
		NodeModel parent,
		PortAlignment alignment = PortAlignment.Bottom,
		Point? position = null,
		Size? size = null)
		: base(parent, alignment, position, size)
	{
		foreach (var l in this.Links)
		{
			l.SourceChanged += (a, b, c) =>
			{
				var dbg = 2;
			};

			l.TargetChanged += (a, b, c) =>
			{
				var dbg = 2;
			};
		}
	}

	public void OnChange(Action<PpNodePort> onChange)
	{
		_onChange = onChange;
	}

	public string Name => PortAttribute.Name;

	public PpPortAttribute PortAttribute { get; set; }

	public Func<PpNodeInput>? GetNodeInput { get; set; }

	public Func<PpNodeOutput>? GetNodeOutput { get; set; }

	public override void Refresh()
	{
		base.Refresh();

		var link = Links.FirstOrDefault();
		if (link == null)
		{
			Console.WriteLine("No links");
			var x1 = GetNodeInput?.Invoke();
			if (x1 != null)
			{
				Console.WriteLine("Disconnected");
				x1.Output = null;
			}

			return;
		}

		Console.WriteLine($"LINKS:{link.Links.Count}");

		if (link.Source?.Model is not PpNodePort src)
		{
			Console.WriteLine("No source");
			return;
		}

		if (link?.Target?.Model is not PpNodePort dst)
		{
			Console.WriteLine("No target");
			return;
		}

		var srcNodeOutput = src.GetNodeOutput?.Invoke();
		if (srcNodeOutput == null)
		{
			Console.WriteLine("No source node");
			return;
		}

		var dstNodeInput = dst.GetNodeInput?.Invoke();
		if (dstNodeInput == null)
		{
			Console.WriteLine("No target node");
			return;
		}

		dstNodeInput.Output = srcNodeOutput;

		Console.WriteLine($"Connected {src} to {dst}");

		_onChange?.Invoke(this);
	}
}