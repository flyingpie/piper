// using Piper.Core.Attributes;
// using Piper.Core.Data;
// using static Piper.Core.Data.PpPortDirection;
//
// namespace Piper.Core.Nodes;
//
// public class PpRegexNode : PpNodeBase
// {
// 	private PpTable _match = new("todo");
// 	private PpTable _noMatch = new("todo");
//
// 	public PpRegexNode()
// 	{
// 		InPaths = new();
//
// 		OutMatch = new() { Table = () => _match, };
// 		OutNoMatch = new() { Table = () => _noMatch, };
// 	}
//
// 	public string NodeType => "Regex";
//
// 	public string Name { get; set; }
//
// 	public bool IsExecuting { get; }
//
// 	[PpParam("Pattern")]
// 	public string? InPattern { get; set; }
//
// 	[PpParam("Attribute")]
// 	public string? InAttribute { get; set; }
//
// 	[PpPort(In, "Input")]
// 	public PpNodeInput InPaths { get; set; }
//
// 	[PpPort(Out, "Match")]
// 	public PpNodeOutput OutMatch { get; }
//
// 	[PpPort(Out, "No Match")]
// 	public PpNodeOutput OutNoMatch { get; }
//
// 	protected override async Task OnExecuteAsync()
// 	{
// 		if (!InPaths.IsConnected)
// 		{
// 			LogWarning($"Port '{InPaths}' not connected, stopping");
// 			return;
// 		}
//
// 		if (string.IsNullOrWhiteSpace(InAttribute))
// 		{
// 			LogWarning("No attribute specified, stopping");
// 			return;
// 		}
//
// 		await _match.ClearAsync();
// 		await _noMatch.ClearAsync();
//
// 		var regex = new Regex(InPattern ?? string.Empty, RegexOptions.Compiled);
//
// 		await foreach (var rec in InPaths.Table().QueryAllAsync())
// 		{
// 			var f = rec.Fields.ToDictionary();
//
// 			var match = regex.Match(f[InAttribute].ValueAsString);
// 			if (match.Success)
// 			{
// 				foreach (var grp in match.Groups.OfType<Group>())
// 				{
// 					if (!char.IsAsciiLetter(grp.Name[0]))
// 					{
// 						continue;
// 					}
//
// 					f[grp.Name] = new(grp.Value);
// 				}
//
// 				var rec2 = new PpRecord() { Fields = f, };
// 				await _match.AddAsync(rec2);
// 			}
// 			else
// 			{
// 				var rec2 = new PpRecord() { Fields = f, };
// 				await _noMatch.AddAsync(rec2);
// 			}
// 		}
// 	}
// }