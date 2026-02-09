using Piper.Core.Attributes;
using Piper.Core.Data;
using static Piper.Core.Data.PpPortDirection;

namespace Piper.Core.Nodes.Unix;

public class PpSMapNode : PpNode
{
	public PpSMapNode()
	{
		OutProcesses = new(this, nameof(OutProcesses), new(PpTable.GetTableName(this, nameof(OutProcesses))));
	}

	[PpParam("Process Name")]
	public string ProcessName { get; set; } = "";

	[PpPort(Out, "Rows")]
	public PpNodeOutput OutProcesses { get; }

	public override bool SupportsProgress { get; }

	protected override async Task OnExecuteAsync()
	{
		var pids = Process.GetProcesses()
		// .Take(5)
		;

		var isInit = false;

		PpDbAppender? appender = null;

		await using (var x = appender)
		{
			foreach (var pid in pids)
			{
				try
				{
					var recs = await GetSMapAsync(pid.Id);
					if (recs.Count == 0)
					{
						continue;
					}

					if (!isInit)
					{
						OutProcesses.Table.Columns = recs.First().Fields.Select(f => new PpColumn(f.Key, PpDataType.PpString)).ToList();
						await OutProcesses.Table.ClearAsync();
						appender = await OutProcesses.Table.CreateAppenderAsync();
						isInit = true;
					}

					appender.AddRange(recs);
					break;
				}
				catch (Exception ex)
				{
					Console.WriteLine($"EXC:{ex.Message}");
				}
			}
		}

		await OutProcesses.Table.DoneAsync();
	}

	private static async Task<List<PpRecord>> GetSMapAsync(int pid)
	{
		var smapPath = $"/proc/{pid}/smaps";

		if (!File.Exists(smapPath))
		{
			return [];
		}

		var res = new List<PpRecord>();

		var lines = await File.ReadAllLinesAsync(smapPath);

		var headerRegex = new Regex(
			@"^(?<addr_from>[0-9a-f]+)-(?<addr_to>[0-9a-f]+) (?<perm>[^ ]+) (?<offset>[0-9a-f]+) (?<device>[0-9a-f]{2}:[0-9a-f]{2}) (?<inode>[0-9]+) +(?<module>.+)$"
		);
		var propRegex = new Regex(@"^(?<name>.+): +(?<val>.+)$");

		PpRecord? rec = null;

		foreach (var line in lines)
		{
			var match = headerRegex.Match(line);
			if (match.Success)
			{
				rec = new();
				rec.Fields["pid"] = pid;
				foreach (Group grp in match.Groups)
				{
					if (grp.Index == 0)
					{
						continue;
					}

					rec.Fields[grp.Name] = grp.Value;
				}

				res.Add(rec);
				continue;
			}

			if (rec == null)
			{
				continue;
			}

			var match2 = propRegex.Match(line);
			if (match2.Success)
			{
				rec.Fields[match2.Groups["name"].Value] = match2.Groups["val"].Value;
			}
		}

		return res;
	}
}
