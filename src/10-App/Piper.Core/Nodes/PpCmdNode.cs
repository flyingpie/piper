using Piper.Core.Attributes;
using Piper.Core.Data;
using static Piper.Core.Data.PpDataType;
using static Piper.Core.Data.PpPortDirection;

namespace Piper.Core.Nodes;

public class PpCmdNode : PpNode
{
	private readonly PpTable _results;

	public PpCmdNode()
	{
		_results = new("results");

		InInputs = new(this, nameof(InInputs));

		OutResults = new(this, nameof(OutResults)) { Table = () => _results };
	}

	public override bool SupportsProgress => true;

	[PpPort(In, "Inputs")]
	public PpNodeInput InInputs { get; }

	[PpParam("FileName")]
	public string? FileName { get; set; }

	[PpPort(Out, "Results")]
	public PpNodeOutput OutResults { get; }

	protected override async Task OnExecuteAsync()
	{
		if (!InInputs.IsConnected)
		{
			Logs.Warning($"Port '{InInputs}' not connected");
			return;
		}

		// if (string.IsNullOrWhiteSpace(InAttr))
		// {
		// 	Logs.Warning($"Param '{InAttr}' not set");
		// 	return;
		// }

		var inputs = InInputs.Output.Table();

		await foreach (var inp in inputs.QueryAllAsync())
		{
			var proc = new ProcessStartInfo()
			{
				FileName = FileName,
				// WorkingDirectory = "",
			};

			proc.ArgumentList.Add(".");
		}
	}
}
