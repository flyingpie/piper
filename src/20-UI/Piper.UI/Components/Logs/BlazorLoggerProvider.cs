using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace Piper.UI.Components.Logs;

public sealed class BlazorLoggerProvider : ILoggerProvider
{
	private readonly Debouncer _debounce = new(300);

	public static BlazorLoggerProvider Instance { get; } = new();

	public Action<BlazorLogEvent>? OnEvent { get; set; }

	public ConcurrentQueue<BlazorLogEvent> LogEvents { get; } = new();

	public void Emit(BlazorLogEvent logEvent)
	{
		LogEvents.Enqueue(logEvent);

		while (LogEvents.Count > 100)
		{
			LogEvents.TryDequeue(out _);
		}

		_debounce.Debounce(() =>
		{
			OnEvent?.Invoke(logEvent);
			return Task.CompletedTask;
		});
	}

	public void Dispose() { }

	public ILogger CreateLogger(string categoryName)
	{
		return new BlazorLogger(Emit, categoryName);
	}
}
