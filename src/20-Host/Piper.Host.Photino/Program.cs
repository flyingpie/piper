using System.Drawing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Photino.Blazor;
using Piper.Core.Utils;
using Piper.UI.Components.Logs;

namespace Piper.UI;

internal static class Program
{
	public static void Main(string[] args)
	{
		var builder = PhotinoBlazorAppBuilder.CreateDefault();

		builder.Services.AddPiper();
		builder.Services.AddSingleton<ILoggerProvider>(BlazorLoggerProvider.Instance);

		builder.RootComponents.Add<App>("app");

		var app = builder.Build();

		Log.Factory = app.Services.GetRequiredService<ILoggerFactory>();

		app.MainWindow.SetLogVerbosity(0).SetSize(new Size(1920, 900)).SetTitle("Piper");

		app.Run();
	}
}
