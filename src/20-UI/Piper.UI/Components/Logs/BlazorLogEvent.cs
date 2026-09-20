using Microsoft.Extensions.Logging;

namespace Piper.UI.Components.Logs;

public class BlazorLogEvent
{
	public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.Now;

	public LogLevel Level { get; set; }

	public string? Category { get; set; }

	public string? Message { get; set; }

	public Exception? Exception { get; set; }
}
