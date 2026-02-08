using Blazor.Diagrams.Core.Models;
using Microsoft.Extensions.FileSystemGlobbing;
using Piper.Core.Attributes;
using Piper.Core.Data;
using Piper.Core.Db;
using static Piper.Core.Data.PpDataType;
using static Piper.Core.Data.PpPortDirection;

namespace Piper.Core.Nodes;

public class PpReadCsvNode : PpNode
{
	private readonly PpTable _outTables;

	public PpReadCsvNode()
	{
		_outTables = new(PpTable.GetTableName(this, nameof(OutTables)));

		InFiles = new(this, nameof(InFiles));
		OutTables = new(this, nameof(OutTables)) { Table = () => _outTables };
	}

	public override string Color => "#8a2828";

	public override string Icon => "fa-regular fa-file-lines";

	public override string NodeType => "Read CSV";

	public override bool SupportsProgress => true;

	// [PpParam("Path")]
	// public string Path { get; set; } = "";
	[PpPort(In, "File Paths")]
	public PpNodeInput InFiles { get; }

	[PpParam("In Attribute")]
	public string? InAttr { get; set; }

	// [PpParam("Has Header Row")]
	// public bool HasHeaderRow { get; set; } = true;

	// [PpParam("Max File Size")]
	// public int MaxFileSize { get; set; } = 2_000_000; // 2MB

	[PpPort(Out, "Tables")]
	public PpNodeOutput OutTables { get; }

	protected override async Task OnExecuteAsync()
	{
		// if (string.IsNullOrWhiteSpace(Path))
		// {
		// 	Logs.Warning($"Param '{nameof(Path)}' not set");
		// 	return;
		// }
		//
		// if (!File.Exists(Path))
		// {
		// 	Logs.Warning($"Param '{nameof(Path)}' points to file '{Path}', which does not exist");
		// 	return;
		// }

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

		// Read in
		var inTable = InFiles.Output.Table();

		// Prep out
		var cols = inTable.Columns.ToList();
		cols.AddRange([
			//
			new("is_successful", PpBool),
			new("table_name", PpString),
			new("row_count", PpInt64),
			new("error", PpString),
		]);
		_outTables.Columns = cols;
		await _outTables.ClearAsync();

		var i = 0;

		{
			await using var appender = await _outTables.CreateAppenderAsync();

			await foreach (var file in inTable.QueryAllAsync())
			{
				Progress = ((float)i) / inTable.Count;

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

				// Read CSV
				var table = new PpTable($"{_outTables.TableName}_{i++}");

				try
				{
					await PpDb.Instance.ExecuteNonQueryAsync(
						$"""
						create or replace table "{table.TableName}"
							as select * from read_csv('{path}', union_by_name = true)
						"""
					);

					await table.InitAsync();
					// await _outTables.InitAsync();

					// appender.Add(CreateRecord(file, isSuccessful: true, table.TableName, table.Count));
					//
					// var nodeOut = new PpNodeOutput(this, table.TableName) { Table = () => table };
					// _dynNodeProps.Add(
					// 	new PpNodePort(this, PortAlignment.Right)
					// 	{
					// 		//
					// 		Name = table.TableName,
					// 		GetNodeOutput = () => nodeOut,
					// 	}
					// );
				}
				catch (Exception ex)
				{
					//
					appender.Add(CreateRecord(file, isSuccessful: false, table.TableName, table.Count, error: ex.Message));
				}
			}
		}

		await _outTables.DoneAsync();
	}

	public PpRecord CreateRecord(PpRecord file, bool isSuccessful, string tableName, long count, string? error = null) =>
		new()
		{
			Fields = new Dictionary<string, PpField>(file.Fields, StringComparer.OrdinalIgnoreCase)
			{
				//
				{ "is_successful", isSuccessful },
				{ "table_name", tableName },
				{ "row_count", count },
				{ "error", error },
			},
		};
}
