using Microsoft.Extensions.Logging;
using Piper.Core.Modifiers;
using Piper.Utils;

namespace Piper.Modifiers;

public class PpReadFilesModifier : PpModifier
{
	private readonly ILogger _log = Log.For<PpReadFilesModifier>();

	public override string Name { get; set; } = "Read Files";

	[PpParam("File Name Field")]
	public string? SrcFieldName { get; set; }

	[PpParam("Destination Field")]
	public string? DstFieldName { get; set; }

	public override async Task ExecuteAsync(IPpTable source, CancellationToken ct = default)
	{
		Guard.Against.Null(source);

		if (string.IsNullOrWhiteSpace(SrcFieldName))
		{
			return;
		}

		if (string.IsNullOrWhiteSpace(DstFieldName))
		{
			return;
		}

		await using var appender = await Table.CreateAppenderAsync(ct);

		await foreach (var rec in source.QueryAllAsync(ct))
		{
			if (!rec.TryGetField(SrcFieldName, out var field))
			{
				await appender.AddAsync(rec.With($"No field named '{SrcFieldName}'"));
				break;
			}

			var path = field.ValueAsString;

			try
			{
				var fileCont = await File.ReadAllTextAsync(path);

				var res = PpRecord.From(rec).With((DstFieldName, fileCont));
				await appender.AddAsync(res);
			}
			catch (Exception ex)
			{
				_log.LogError(ex, "Error reading file: {Message}", ex.Message);
				Logs.Error($"Error reading file at path '{path}': {ex.Message}");
			}
		}

		await Table.DoneAsync(ct);
	}
}
