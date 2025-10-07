using Microsoft.Extensions.Logging;

namespace Piper.Core.Nodes;

public class PpLog
{
	public LogLevel Level { get; set; }

	public string Message { get; set; }
}