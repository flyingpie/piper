using Photino.Blazor;
using System.Drawing;

namespace Piper.UI;

internal static class Program
{
	public static void Main(string[] args)
	{
		var appBuilder = PhotinoBlazorAppBuilder.CreateDefault();

		appBuilder.Services.AddPiper();

		appBuilder.RootComponents.Add<App>("app");

		var app = appBuilder.Build();

		app.MainWindow.SetLogVerbosity(0)
			.SetSize(new Size(1920, 1080))
			// .SetIconFile(WtqPaths.GetPathRelativeToWtqAppDir("assets", "icon-v2-64.png"))
			.SetTitle("Piper.Core");

		app.Run();
	}
}
