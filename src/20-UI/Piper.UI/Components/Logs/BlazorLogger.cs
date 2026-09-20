using Microsoft.Extensions.Logging;

namespace Piper.UI.Components.Logs;

public class BlazorLogger(Action<BlazorLogEvent> onEvent, string? categoryName) : ILogger
{
	private readonly Action<BlazorLogEvent> _onEvent = Guard.Against.Null(onEvent);

	public void Log<TState>(
		LogLevel logLevel,
		EventId eventId,
		TState state,
		Exception? exception,
		Func<TState, Exception?, string> formatter
	)
	{
		_onEvent(
			new()
			{
				Category = categoryName,
				Exception = exception,
				Level = logLevel,
				Message = formatter(state, exception),
			}
		);
	}

	public bool IsEnabled(LogLevel logLevel)
	{
		return true;
	}

	public IDisposable? BeginScope<TState>(TState state)
		where TState : notnull
	{
		return null;
	}
}
