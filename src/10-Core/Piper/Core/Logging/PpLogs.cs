using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace Piper.Core.Logging;

public class PpLogs
{
	private readonly ILogger _log = global::Piper.Core.Utils.Log.For<PpLogs>();

	private readonly ConcurrentQueue<PpLog> _logs = [];

	public IReadOnlyCollection<PpLog> Logs => _logs;

	public void Clear() => _logs.Clear();

	public void Log(LogLevel level, string message)
	{
		_log.Log(level, message);

		_logs.Enqueue(new(level, message));
	}

	public void Debug(string message) => Log(LogLevel.Debug, message);

	public void Error(string message) => Log(LogLevel.Error, message);

	public void Info(string message) => Log(LogLevel.Information, message);

	public void Warning(string message) => Log(LogLevel.Warning, message);
}
