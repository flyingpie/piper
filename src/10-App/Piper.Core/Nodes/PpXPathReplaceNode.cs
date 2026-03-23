using System.Collections;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Xml.Linq;
using System.Xml.XPath;
using Piper.Core.Attributes;
using Piper.Core.Data;
using Piper.Core.Utils;
using static Piper.Core.Data.PpDataType;
using static Piper.Core.Data.PpPortDirection;

namespace Piper.Core.Nodes;

public class PpXPathReplaceNode : PpNode
{
	public PpXPathReplaceNode()
	{
		InStuff = new(this, nameof(InStuff));
		// OutResponses = new(this, nameof(OutResponses));
	}

	public override string Color => "#326590";

	public override string Icon => "fa-solid fa-paper-plane";

	public override string NodeType => "XPath Replace";

	public override bool SupportsProgress => true;

	[PpPort(In, "Requests")]
	public PpNodeInput InStuff { get; }

	[PpParam("File Path Attr")]
	public string? FilePathAttr { get; set; }

	[PpParam("XPath Attr")]
	public string? XPathAttr { get; set; }

	[PpParam("Replacement Attr")]
	public string? ReplacementAttr { get; set; }

	// [PpPort(Out, "Responses")]
	// public PpNodeOutput OutResponses { get; }

	protected override async Task OnExecuteAsync()
	{
		if (!InStuff.IsConnected)
		{
			Logs.Warning($"Port '{InStuff}' not connected");
			return;
		}

		if (string.IsNullOrWhiteSpace(FilePathAttr))
		{
			Logs.Warning($"Param '{FilePathAttr}' not set");
			return;
		}

		var inTable = InStuff.Output.Table;

		// var cols = inTable.Columns.ToList();
		// cols.AddRange([new(PpString, "resp")]);
		// OutResponses.BaseTable.Columns = cols;
		// await OutResponses.BaseTable.ClearAsync();

		// var http = new HttpClient();

		{
			// await using var appender = await OutResponses.BaseTable.CreateAppenderAsync();

			var i = 0;
			await foreach (var file in inTable.QueryAllAsync())
			{
				Progress = ((float)i) / inTable.Count;

				if (i % 1000 == 0)
				{
					Changed();
				}

				// Get attribute
				var inFilePath = file.Fields.FirstOrDefault(f => f.Key?.Equals(FilePathAttr, StringComparison.OrdinalIgnoreCase) ?? false);
				var inXPath = file.Fields.FirstOrDefault(f => f.Key?.Equals(XPathAttr, StringComparison.OrdinalIgnoreCase) ?? false);
				var inRepl = file.Fields.FirstOrDefault(f => f.Key?.Equals(ReplacementAttr, StringComparison.OrdinalIgnoreCase) ?? false);

				var fileCont = await File.ReadAllTextAsync(inFilePath.Value.ValueAsString);
				var fileX = XElement.Parse(fileCont);

				foreach (var item in ((IEnumerable)fileX.XPathEvaluate(inXPath.Value.ValueAsString)))
				{
					if (item is XAttribute attr)
					{
						attr.SetValue(inRepl.Value.ValueAsString);
					}
				}

				await File.WriteAllTextAsync(inFilePath.Value.ValueAsString, fileX.ToString());

				var dbg = 2;
				// Logs.Info($"");

				// if (string.IsNullOrWhiteSpace(field.Value?.ValueAsString))
				// {
				// 	Logs.Warning($"Record does not have an attribute with name '{FilePathAttr}'");
				// 	appender.Add(CreateRecord(file, -1, string.Empty));
				// 	continue;
				// }

				// // Read file
				// var path = field.Value.ValueAsString;
				// if (!File.Exists(path))
				// {
				// 	Logs.Warning($"File at path '{path}' does not exist");
				// 	appender.Add(CreateRecord(file, -1, string.Empty));
				// 	continue;
				// }

				// var resp = await http.GetAsync(field.Value.ValueAsString);
				// var respStr = await resp.Content.ReadAsStringAsync(); // TODO: If JSON => Proper result structure
				// var json = PpJson.Deserialize<JsonNode>(respStr);

				// Logs.Info($"({i++}/{9999}) Reading file at path");

				// var recOut = new PpRecord()
				// {
				// 	Fields = new Dictionary<string, PpField>(file.Fields, StringComparer.OrdinalIgnoreCase)
				// 	{
				// 		// { "idx", $"{idx}" },
				// 		{ "resp", PpJson.SerializeToString(new
				// 			{
				// 				status_code = resp.StatusCode,
				// 				response = json
				// 			})
				// 		},
				// 	},
				// };

				// appender.Add(recOut);

				// 	var text = await File.ReadAllTextAsync(path);
				// 	appender.Add(CreateRecord(file, 0, text));
			}
		}

		// await OutResponses.BaseTable.DoneAsync();
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

