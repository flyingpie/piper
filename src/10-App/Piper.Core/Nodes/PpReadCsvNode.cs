using Blazor.Diagrams.Core.Models;
using Piper.Core.Attributes;
using Piper.Core.Data;
using Piper.Core.Db;
using static Piper.Core.Data.PpDataType;
using static Piper.Core.Data.PpPortDirection;

namespace Piper.Core.Nodes;

public class PpReadCsvNode : PpNode
{
	public PpReadCsvNode()
	{
		InFiles = new(this, nameof(InFiles));
		OutTables = new(this, nameof(OutTables), new(PpTable.GetTableName(this, nameof(OutTables))));
	}

	public override string Color => "#8a2828";

	public override string Icon => "fa-regular fa-file-lines";

	public override string NodeType => "Read CSV";

	public override bool SupportsProgress => true;

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
		var inTable = InFiles.Output.Table;

		// Prep out
		var cols = inTable.Columns.ToList();
		cols.AddRange([new("is_successful", PpBool), new("table_name", PpString), new("row_count", PpInt64), new("error", PpString)]);
		OutTables.Table.Columns = cols;
		await OutTables.Table.ClearAsync();

		_dynNodeProps.Clear();

		var i = 0;

		{
			await using var appender = await OutTables.Table.CreateAppenderAsync();

			await foreach (var file in inTable.QueryAllAsync())
			{
				Progress = ((float)i) / inTable.Count;

				// Get attribute
				var field = file.Fields.FirstOrDefault(f => f.Key?.Equals(InAttr, StringComparison.OrdinalIgnoreCase) ?? false);

				if (string.IsNullOrWhiteSpace(field.Value?.ValueAsString))
				{
					// TODO: Send to "skipped" output
					Logs.Warning($"Record does not have an attribute with name '{InAttr}'");
					continue;
				}

				// Read file
				var path = field.Value.ValueAsString;
				if (!File.Exists(path))
				{
					// TODO: Send to "skipped" output
					Logs.Warning($"File at path '{path}' does not exist");
					continue;
				}

				// Read CSV
				var table = new PpTable($"{OutTables.Table.TableName}_{i++}");

				try
				{
					await PpDb.Instance.ExecuteNonQueryAsync(
						$"""
						create or replace table "{table.TableName}"
							as select * from read_csv('{path}', union_by_name = true)
						"""
					);

					await table.InitAsync();

					appender.Add(CreateRecord(file, isSuccessful: true, table.TableName, table.Count));

					var nodeOut = new PpNodeOutput(this, table.TableName, table);
					_dynNodeProps.Add(new PpNodePort(table.TableName, this, PortAlignment.Right) { GetNodeOutput = () => nodeOut });
				}
				catch (Exception ex)
				{
					appender.Add(CreateRecord(file, isSuccessful: false, table.TableName, table.Count, error: ex.Message));
				}
			}
		}

		await OutTables.Table.DoneAsync();
	}

	private static PpRecord CreateRecord(PpRecord file, bool isSuccessful, string tableName, long count, string? error = null) =>
		new()
		{
			Fields = new Dictionary<string, PpField>(file.Fields, StringComparer.OrdinalIgnoreCase)
			{
				{ "is_successful", isSuccessful },
				{ "table_name", tableName },
				{ "row_count", count },
				{ "error", error },
			},
		};
}
