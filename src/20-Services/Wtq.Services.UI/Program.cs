using Microsoft.Extensions.DependencyInjection;
using Photino.Blazor;

namespace Wtq.Services.UI;

public static class Program
{
	public static async Task Main(string[] args)
	{
		var appBuilder = PhotinoBlazorAppBuilder.CreateDefault();

		// TODO: Unify with the main app DI.
		appBuilder.Services
			.AddUI()
			.AddLogging();

		appBuilder.RootComponents.Add<App>("app");

		var _app = appBuilder.Build();

		_app.MainWindow

			// .SetIconFile(WtqPaths.GetPathRelativeToWtqAppDir("assets", "icon-v2-64.png"))
			.SetTitle("Grizzly");

//		_app.MainWindow.RegisterWindowCreatedHandler((s, a) => { _ = Task.Run(CloseMainWindowAsync); });

		_app.Run();
	}
}