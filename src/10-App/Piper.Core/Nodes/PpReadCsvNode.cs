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

		OutRecords = new(this, nameof(OutRecords), new(PpTable.GetTableName(this, nameof(OutRecords))));
		OutFailures = new(this, nameof(OutFailures), new(PpTable.GetTableName(this, nameof(OutFailures))));
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

	[PpPort(Out, "Records")]
	public PpNodeOutput OutRecords { get; }

	[PpPort(Out, "Failures")]
	public PpNodeOutput OutFailures { get; }

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
		var cols1 = inTable.Columns.ToList();
		cols1.AddRange([new("csv_row", PpString)]);
		OutRecords.Table.Columns = cols1;
		await OutRecords.Table.ClearAsync();

		var cols2 = inTable.Columns.ToList();
		cols2.AddRange([new("error", PpString)]);
		OutFailures.Table.Columns = cols2;
		await OutFailures.Table.ClearAsync();

		var i = 0;

		{
			await using var appender = await OutRecords.Table.CreateAppenderAsync();
			await using var appender2 = await OutFailures.Table.CreateAppenderAsync();

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
				try
				{
					await foreach (
						var row in PpDb.Instance.RawQueryAsync(
							$"""
								select row_to_json(csv) as csv_row from read_csv('{path}', union_by_name = true) as csv
							"""
						)
					)
					{
						appender.Add(CreateRecord(file, row.Fields.First().Value.ValueAsString));
					}
				}
				catch (Exception ex)
				{
					// appender.Add(CreateRecord(file, isSuccessful: false, null, error: ex.Message));

					appender2.Add(
						new PpRecord()
						{
							Fields = new Dictionary<string, PpField>(file.Fields, StringComparer.OrdinalIgnoreCase)
							{
								{ "error", new(PpString, ex.Message) },
							},
						}
					);
				}

				// 	var csvConf = new CsvConfiguration(CultureInfo.InvariantCulture)
				// 	{
				// 		//
				// 		HasHeaderRecord = true,
				// 		DetectDelimiter = true,
				// 	};
				// 	using var fileStr = File.OpenRead(path);
				// 	using var strReader = new StreamReader(fileStr);
				// 	using var csvReader = new CsvHelper.CsvReader(strReader, csvConf);
				//
				// 	if (!await csvReader.ReadAsync())
				// 	{
				// 		//
				// 	}
				//
				// 	csvReader.ReadHeader();
				//
				// 	while (await csvReader.ReadAsync())
				// 	{
				// 		// var dict = new Dictionary<string, PpField>();
				// 		var dict = new Dictionary<string, string?>();
				//
				// 		foreach (var h in csvReader.HeaderRecord ?? [])
				// 		{
				// 			if (csvReader.TryGetField(typeof(string), h, out var val))
				// 			{
				// 				// dict[h] = new PpField(PpString, val);
				// 				dict[h] = val?.ToString();
				//
				// 				var xy = 2;
				// 			}
				// 			else
				// 			{
				// 				dict[h] = null;
				//
				// 				var xy = 2;
				// 			}
				// 		}
				// 		//
				//
				// 		// var x = csvReader.GetRecord<Dictionary<string, object>>();
				//
				// 		appender.Add(CreateRecord(file, isSuccessful: true, Utils.PpJson.SerializeToString(dict), error: null));
				//
				// 		var xx = 2;
				// 	}
			}
		}

		await OutRecords.Table.DoneAsync();
		await OutFailures.Table.DoneAsync();
	}

	private static PpRecord CreateRecord(PpRecord file, string? csvRow) =>
		new()
		{
			Fields = new Dictionary<string, PpField>(file.Fields, StringComparer.OrdinalIgnoreCase)
			{
				{ "csv_row", new(PpString, csvRow) },
			},
		};
}
