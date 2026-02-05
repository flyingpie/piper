using Microsoft.Extensions.Logging;

namespace Piper.Core.Logging;

public class PpLog(LogLevel level, string message)
{
	public LogLevel Level { get; } = level;

	public string Message { get; } = Guard.Against.NullOrWhiteSpace(message);
}
