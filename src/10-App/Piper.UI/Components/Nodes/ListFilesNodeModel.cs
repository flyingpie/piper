using System.Collections.Generic;
using System.Diagnostics;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Newtonsoft.Json;

namespace Piper.UI.Components.Nodes;

public class ListFilesNodeModel : NodeModel
{
	public ListFilesNodeModel(SelectedThingyService selected, Point position)
		: base(position)
	{
		SelectedThingy = selected;

		StdIn = this.AddPort(new PortModel(id: "stdin", parent: this, PortAlignment.Left));

		// var p2 = this.AddPort(new PortModel(id: "", parent: this, PortAlignment.Left));

		StdOut = this.AddPort(new PortModel(id: "stdout", parent: this, PortAlignment.Right));
		StdErr = this.AddPort(new PortModel(id: "stderr", parent: this, PortAlignment.Right));
	}

	// public double FirstNumber { get; set; }
	//
	// public double SecondNumber { get; set; }

	public SelectedThingyService SelectedThingy { get; set; }

	[JsonProperty]
	public string Command { get; set; }

	[JsonProperty]
	public List<CmdArgument> Args { get; set; } = [];

	public class CmdArgument
	{
		public string? Arg { get; set; }
	}

	public PiperDataFrame StdInData { get; set; }

	public PiperDataFrame StdOutData { get; set; } =
		new()
		{
			Records =
			[
				new PiperRecord() { Fields = [new PiperField() { Value = "PF1" }] },
				new PiperRecord() { Fields = [new PiperField() { Value = "PF2" }] },
				new PiperRecord() { Fields = [new PiperField() { Value = "PF3" }] },
				new PiperRecord() { Fields = [new PiperField() { Value = "PF4" }] },
			],
		};

	public PiperDataFrame StdErrData { get; set; }

	public PortModel StdIn { get; set; }

	public PortModel StdOut { get; set; }

	public PortModel StdErr { get; set; }

	public bool IsExecuting { get; set; }

	public async Task ExecuteAsync()
	{
		IsExecuting = true;

		try
		{
			Console.WriteLine($"CMD:{Command} ARGS:{Args}");

			var srcData = (
				(StdIn.Links.FirstOrDefault()?.Source?.Model as PortModel)?.Parent
				as ListFilesNodeModel
			)?.StdOutData;

			var fr = new PiperDataFrame();
			var err = new List<string>();

			var p = new Process();

			p.StartInfo.FileName = Command;
			// p.StartInfo.Arguments = Args;
			foreach (var arg in Args)
			{
				p.StartInfo.ArgumentList.Add(arg.Arg);
			}

			p.StartInfo.UseShellExecute = false;

			p.StartInfo.RedirectStandardError = true;
			p.StartInfo.RedirectStandardInput = true;
			p.StartInfo.RedirectStandardOutput = true;

			p.OutputDataReceived += (s, a) =>
				fr.Records.Add(
					new PiperRecord() { Fields = [new PiperField() { Value = a.Data }] }
				);
			p.ErrorDataReceived += (s, a) => err.Add($"ERR:{a.Data}");

			// p.RedirectStandardError = true;
			// p.RedirectStandardInput = true;
			// p.RedirectStandardOutput = true;

			// var pp = Process.Start(p);

			// while (true)
			// {
			// 	var line = await pp.StandardOutput.ReadLineAsync();
			// 	if (line == null)
			// 	{
			// 		break;
			// 	}
			//
			// 	fr.Records.Add(new PiperRecord() { Fields = [new PiperField() { Value = line }] });
			// }

			p.Start();

			p.BeginErrorReadLine();
			p.BeginOutputReadLine();

			foreach (var line in srcData?.Records ?? [])
			{
				await p.StandardInput.WriteLineAsync(
					line.Fields?.FirstOrDefault()?.Value?.ToString() ?? ""
				);
			}

			await p.StandardInput.FlushAsync();
			p.StandardInput.Close();

			await p.WaitForExitAsync();

			StdOutData = fr;
			SelectedThingy.Node = fr;

			// await Task.Delay(TimeSpan.FromSeconds(2));
		}
		catch (Exception ex)
		{
			Console.WriteLine($"OW NOES: {ex.Message}");
		}

		Console.WriteLine($"COUNT:{StdOutData?.Records?.Count ?? -1}");

		IsExecuting = false;
	}
}

public class PiperPortModel : PortModel
{
	public PiperPortModel(
		NodeModel parent,
		PortAlignment alignment = PortAlignment.Bottom,
		Point? position = null,
		Size? size = null
	)
		: base(parent, alignment, position, size) { }

	public PiperPortModel(
		string id,
		NodeModel parent,
		PortAlignment alignment = PortAlignment.Bottom,
		Point? position = null,
		Size? size = null
	)
		: base(id, parent, alignment, position, size) { }
}
