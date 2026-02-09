using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Piper.Core.Data;

namespace Piper.Core;

/// <summary>
/// Subclasses BlazorDiagram's <see cref="PortModel"/>, to add sync between the diagram model and the Piper one
/// (the one that actually executed things and is serialized).
/// </summary>
public class PpNodePort(
	string name,
	NodeModel parent,
	PortAlignment alignment = PortAlignment.Bottom,
	Point? position = null,
	Size? size = null
) : PortModel(parent, alignment, position, size), IPpNodeProperty
{
	private Action<PpNodePort>? _onChange;

	public string Name { get; } = Guard.Against.NullOrWhiteSpace(name);

	public Func<PpNodeInput>? GetNodeInput { get; set; }

	public Func<PpNodeOutput>? GetNodeOutput { get; set; }

	// public PpNodeInput? NodeInput { get; init; }

	// public PpNodeOutput? NodeOutput { get; init; }

	public PpTable? Table => GetNodeInput?.Invoke()?.Output?.Table ?? GetNodeOutput?.Invoke()?.Table;

	public bool IsConnected => Links.Count > 0;

	public void OnChange(Action<PpNodePort> onChange)
	{
		_onChange = onChange;
	}

	public void DisconnectAll()
	{
		// foreach (var link in Links)
		// {
		// 	Console.WriteLine($"LINKS:{link.Links.Count}");
		//
		// 	if (link.Source?.Model is not PpNodePort src)
		// 	{
		// 		Console.WriteLine("No source");
		// 		continue;
		// 	}
		//
		// 	if (link?.Target?.Model is not PpNodePort dst)
		// 	{
		// 		Console.WriteLine("No target");
		// 		continue;
		// 	}
		//
		// 	var srcNodeOutput = GetNodeOutput?.Invoke();
		// 	if (srcNodeOutput == null)
		// 	{
		// 		Console.WriteLine("No source node");
		// 		continue;
		// 	}
		//
		// 	var dstNodeInput = GetNodeInput?.Invoke();
		// 	if (dstNodeInput == null)
		// 	{
		// 		Console.WriteLine("No target node");
		// 		continue;
		// 	}
		//
		// 	dstNodeInput.Output = null;
		//
		// 	link.Diagram!.Links.Remove(link);
		//
		// 	Console.WriteLine($"Disconnected {src} from {dst}");
		// }
		//
		// _onChange?.Invoke(this);

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

		// dstNodeInput.Output = srcNodeOutput;
		//
		// Console.WriteLine($"Connected {src} to {dst}");

		dstNodeInput.Output = null;

		link.Diagram!.Links.Remove(link);

		_onChange?.Invoke(this);
	}

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
