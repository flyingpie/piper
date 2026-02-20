using ElectronNET.API;
using ElectronNET.API.Entities;

namespace Piper.Host.Electron;

public static class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		builder.Services.AddRazorComponents().AddInteractiveWebAssemblyComponents();

		builder.Services.AddElectron(); // <-- might be useful to set up DI

		builder.UseElectron(
			args,
			async () =>
			{
				var options = new BrowserWindowOptions
				{
					Show = false,
					IsRunningBlazor = true, // <-- crucial
				};

				if (OperatingSystem.IsWindows() || OperatingSystem.IsLinux())
				{
					options.AutoHideMenuBar = true;
				}

				var browserWindow = await global::ElectronNET.API.Electron.WindowManager.CreateWindowAsync(options);
				browserWindow.OnReadyToShow += () => browserWindow.Show();
			}
		);

		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
		{
			app.UseWebAssemblyDebugging();
		}
		else
		{
			app.UseExceptionHandler("/Error", createScopeForErrors: true);
		}

		app.UseStaticFiles();
		app.UseAntiforgery();

		app.MapRazorComponents<Piper.UI.App>().AddInteractiveWebAssemblyRenderMode();

		app.Run();
	}
}
