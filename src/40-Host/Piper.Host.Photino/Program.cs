using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Photino.Blazor;
using Piper.Core;
using Piper.Core.Utils;
using Piper.UI.Components.Logs;

namespace Piper.UI;

internal static class Program
{
	public static async Task<int> Main(string[] args)
	{
		var builder = PhotinoBlazorApp.CreateBuilder(args);

		builder.Services.AddPiper();
		builder.Services.AddSingleton<ILoggerProvider>(BlazorLoggerProvider.Instance);

		builder.RootComponents.Add<App>("app");

		var app = builder.Build();

		Log.Factory = app.Services.GetRequiredService<ILoggerFactory>();

		await using var piperApp = app;

		piperApp.MainWindow.SetLogVerbosity(0).SetSize(1920, 900).SetTitle("Piper");

		piperApp.Run();

		return 0;
	}
}