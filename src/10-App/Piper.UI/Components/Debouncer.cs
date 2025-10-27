namespace Piper.UI.Components;

public sealed class Debouncer(int delayMilliseconds = 300) : IDisposable
{
	private readonly int _delayMilliseconds = Guard.Against.OutOfRange(delayMilliseconds, nameof(delayMilliseconds), 0, 15_000);
	private CancellationTokenSource _cancellationTokenSource = null!;

	public void Debounce(Func<Task> action)
	{
		_cancellationTokenSource?.Cancel();
		_cancellationTokenSource = new CancellationTokenSource();

		var token = _cancellationTokenSource.Token;

		_ = Task.Run(
			async () =>
			{
				try
				{
					await Task.Delay(_delayMilliseconds, token);

					if (!token.IsCancellationRequested)
					{
						await action();
					}
				}
				catch (TaskCanceledException)
				{
					// Ignore — debounce was reset
				}
			},
			token
		);
	}

	public void Dispose()
	{
		_cancellationTokenSource?.Dispose();
	}
}
