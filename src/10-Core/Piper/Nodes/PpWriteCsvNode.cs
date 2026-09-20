using System.Globalization;
using CsvHelper;
using Piper.Core.Attributes;
using Piper.Core.Data;

namespace Piper.Core.Nodes;

public class PpWriteCsvNode : PpNode
{
	public PpWriteCsvNode()
	{
		InRecords = new(this, nameof(InRecords));
	}

	public override string Color => "#8a2828";

	public override string Icon => "fa-regular fa-file-lines";

	public override string NodeType => "Write CSV";

	public override bool SupportsProgress => true;

	[PpParam("Path")]
	public string? InPathAttr { get; set; }

	[PpPort(PpPortDirection.In, "Records")]
	public PpNodeInput InRecords { get; }

	protected override async Task OnExecuteAsync()
	{
		if (!InRecords.IsConnected)
		{
			Logs.Warning($"Port '{InRecords}' not connected");
			return;
		}

		// Read in
		var inTable = InRecords.Table;

		var i = 0;

		try
		{
			await foreach (var file in inTable.QueryAllAsync())
			{
				Progress = ((float)i) / inTable.Count;

				// Get attribute
				var field = file.Fields.FirstOrDefault(f => f.Key?.Equals(InPathAttr, StringComparison.OrdinalIgnoreCase) ?? false);

				if (string.IsNullOrWhiteSpace(field.Value.ValueAsString))
				{
					continue;
				}

				var wr = GetCsvWriter(field.Value.ValueAsString);

				await wr.NextRecordAsync();
				foreach (var f in file.Fields)
				{
					wr.WriteField(f.Value.Value);
				}
			}
		}
		finally
		{
			foreach (var wr in _writers)
			{
				await wr.Value.DisposeAsync();
			}

			_writers.Clear();
		}
	}

	private readonly Dictionary<string, CsvWriter> _writers = new();

	private CsvWriter GetCsvWriter(string path)
	{
		var fs = File.OpenWrite(path);
		var wr = new StreamWriter(fs);
		var csv = new CsvWriter(wr, CultureInfo.InvariantCulture);

		return csv;
	}
}
