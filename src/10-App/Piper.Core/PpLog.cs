using Microsoft.Extensions.Logging;

namespace Piper.Core;

public class PpLog
{
	public LogLevel Level { get; set; }

	public string Message { get; set; }
}