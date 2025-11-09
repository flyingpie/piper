using Blazor.Diagrams.Core.Models;
using CsvHelper;
using CsvHelper.Configuration;
using Piper.Core.Attributes;
using Piper.Core.Data;
using Piper.Core.Db;
using System.Globalization;
using static Piper.Core.Data.PpDataType;
using static Piper.Core.Data.PpPortDirection;

namespace Piper.Core.Nodes;

public class PpReadCsvNode : PpNode
{
	private readonly PpTable _outLines;

	public PpReadCsvNode()
	{
		_outLines = new(PpTable.GetTableName(this, nameof(OutLines)));

		// InFiles = new(this, nameof(InFiles));
		OutLines = new(this, nameof(OutLines)) { Table = () => _outLines, };
	}

	public override string Color => "#8a2828";

	public override string Icon => "fa-regular fa-file-lines";

	public override string NodeType => "Read CSV";

	public override bool SupportsProgress => true;

	[PpPort(In, "File Paths")]
	public PpNodeInput InFiles { get; }

	[PpParam("File Path Attribute")]
	public string? InAttr { get; set; }

	// [PpParam("Path Pattern")]
	// public string PathPattern { get; set; } = "";

	[PpParam("Has Header Row")]
	public bool HasHeaderRow { get; set; } = true;

	[PpParam("Max File Size")]
	public int MaxFileSize { get; set; } = 2_000_000; // 2MB

	// [PpParam("Max Schemas")]
	// public int MaxSchemas { get; set; } = 10;

	[PpPort(Out, "Lines")]
	public PpNodeOutput OutLines { get; }

	protected override async Task OnExecuteAsync()
	{
		if (!InFiles.IsConnected)
		{
			Logs.Warning($"Port '{InFiles}' not connected");
			return;
		}

		if (string.IsNullOrWhiteSpace(InAttr))
		{
			Logs.Warning($"Param '{InAttr}' not set");
			return;
		}

		var inTable = InFiles.Output.Table();

		await foreach (var file in inTable.QueryAllAsync())
		{
			// Get attribute
			var field = file.Fields.FirstOrDefault(f => f.Key?.Equals(InAttr, StringComparison.OrdinalIgnoreCase) ?? false);

			if (string.IsNullOrWhiteSpace(field.Value?.ValueAsString))
			{
				Logs.Warning($"Record does not have an attribute with name '{InAttr}'");
				// TODO: Send to "skipped" output
				continue;
			}

			// Read file
			var path = field.Value.ValueAsString;
			if (!File.Exists(path))
			{
				Logs.Warning($"File at path '{path}' does not exist");
				// TODO: Send to "skipped" output
				continue;
			}

			await using var fileStream = File.OpenRead(path);
			using var streamReader = new StreamReader(fileStream);
			using var csvReader = new CsvReader(streamReader, new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				BadDataFound = null,
			});

			// while ()
		}

		// var paths = await GetFilePathsAsync(inTable);
		// var pathsStr = string.Join(", ", paths.Select(p => $"'{p}'"));
		// await PpDb.Instance.V_NonQueryRawAsync($"create table \"{_outLines.TableName}\" as select * from read_csv('{PathPattern}', union_by_name = true)");
		// await _outLines.InitAsync();

		// _dynNodeProps.Add(new PpNodePort(this, PortAlignment.Right) { Name = $"Dyn Port {_dynNodeProps.Count}" });
	}
}