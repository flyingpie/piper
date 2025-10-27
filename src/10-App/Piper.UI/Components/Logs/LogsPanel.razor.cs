using Microsoft.Extensions.Logging;

namespace Piper.UI.Components.Logs;

public partial class LogsPanel
{
	private readonly IEnumerable<LogLevel> _levels =
	[
		LogLevel.Critical,
		LogLevel.Error,
		LogLevel.Warning,
		LogLevel.Information,
		// LogLevel.Debug,
		// LogLevel.Verbose,
	];

	private IEnumerable<BlazorLogEvent> EventsFiltered =>
		BlazorLoggerProvider.Instance.LogEvents.Where(l => _levels.Contains(l.Level)).OrderByDescending(l => l.Timestamp);

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();

		BlazorLoggerProvider.Instance.OnEvent = ev =>
		{
			_ = InvokeAsync(() =>
			{
				StateHasChanged();
				return Task.CompletedTask;
			});
		};
	}

	private static string GetBackgroundClass(LogLevel level) =>
		level switch
		{
			LogLevel.Critical		=> "rz-background-color-danger-dark",
			LogLevel.Error			=> "rz-background-color-danger",
			LogLevel.Warning		=> "rz-background-color-warning",
			LogLevel.Information	=> "rz-background-color-primary-light",
			LogLevel.Debug			=> "rz-background-color-base-700",
			LogLevel.Trace			=> "rz-background-color-base-800",
			_ => string.Empty,
		};
}