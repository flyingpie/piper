using System.Net.Http;
using System.Text.Json.Nodes;
using Piper.Core.Attributes;
using Piper.Core.Data;
using Piper.Core.Utils;
using static Piper.Core.Data.PpDataType;
using static Piper.Core.Data.PpPortDirection;

namespace Piper.Core.Nodes;

public class PpHttpNode : PpNode
{
	public PpHttpNode()
	{
		InRequests = new(this, nameof(InRequests));
		OutResponses = new(this, nameof(OutResponses));
	}

	public override string Color => "#326590";

	public override string Icon => "fa-solid fa-paper-plane";

	public override string NodeType => "HTTP";

	public override bool SupportsProgress => true;

	[PpPort(In, "Requests")]
	public PpNodeInput InRequests { get; }

	[PpParam("In Attribute")]
	public string? InAttr { get; set; }

	// [PpParam("Max File Size")]
	// public int MaxFileSize { get; set; } = 2_000_000; // 2MB

	// [PpParam("Split Lines")]
	// public bool SplitLines { get; set; }

	[PpPort(Out, "Responses")]
	public PpNodeOutput OutResponses { get; }

	protected override async Task OnExecuteAsync()
	{
		if (!InRequests.IsConnected)
		{
			Logs.Warning($"Port '{InRequests}' not connected");
			return;
		}

		if (string.IsNullOrWhiteSpace(InAttr))
		{
			Logs.Warning($"Param '{InAttr}' not set");
			return;
		}

		var inTable = InRequests.Output.Table;

		var cols = inTable.Columns.ToList();
		cols.AddRange([new(PpString, "resp")]);
		OutResponses.BaseTable.Columns = cols;
		await OutResponses.BaseTable.ClearAsync();

		var http = new HttpClient();

		{
			await using var appender = await OutResponses.BaseTable.CreateAppenderAsync();

			var i = 0;
			await foreach (var file in inTable.QueryAllAsync())
			{
				Progress = ((float)i) / inTable.Count;

				if (i % 1000 == 0)
				{
					Changed();
				}

				// Get attribute
				var field = file.Fields.FirstOrDefault(f => f.Key?.Equals(InAttr, StringComparison.OrdinalIgnoreCase) ?? false);

				if (string.IsNullOrWhiteSpace(field.Value?.ValueAsString))
				{
					Logs.Warning($"Record does not have an attribute with name '{InAttr}'");
					appender.Add(CreateRecord(file, -1, string.Empty));
					continue;
				}

				// // Read file
				// var path = field.Value.ValueAsString;
				// if (!File.Exists(path))
				// {
				// 	Logs.Warning($"File at path '{path}' does not exist");
				// 	appender.Add(CreateRecord(file, -1, string.Empty));
				// 	continue;
				// }

				var resp = await http.GetAsync(field.Value.ValueAsString);
				var respStr = await resp.Content.ReadAsStringAsync(); // TODO: If JSON => Proper result structure
				var json = PpJson.Deserialize<JsonNode>(respStr);

				Logs.Info($"({i++}/{9999}) Reading file at path");

				var recOut = new PpRecord()
				{
					Fields = new Dictionary<string, PpField>(file.Fields, StringComparer.OrdinalIgnoreCase)
					{
						// { "idx", $"{idx}" },
						{ "resp", PpJson.SerializeToString(new
							{
								status_code = resp.StatusCode,
								response = json
							})
						},
					},
				};

				appender.Add(recOut);

				// 	var text = await File.ReadAllTextAsync(path);
				// 	appender.Add(CreateRecord(file, 0, text));
			}
		}

		await OutResponses.BaseTable.DoneAsync();
	}

	public PpRecord CreateRecord(PpRecord file, int idx, string line) =>
		new()
		{
			Fields = new Dictionary<string, PpField>(file.Fields, StringComparer.OrdinalIgnoreCase)
			{
				// { "idx", $"{idx}" },
				{ "resp", line },
			},
		};
}

