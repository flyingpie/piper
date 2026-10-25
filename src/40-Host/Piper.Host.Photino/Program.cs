using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Photino.Blazor;
using Piper.UI;
using Piper.UI.Components.Logs;
using Piper.Utils;
#if HOTRELOAD
using Dtr.Blazor.HotReload;
#endif

namespace Piper.Host.Photino;

internal static class Program
{
	public static async Task<int> Main(string[] args)
	{
		var builder = PhotinoBlazorApp.CreateBuilder(args);

		builder.Services.AddSingleton<ILoggerProvider>(BlazorLoggerProvider.Instance);

		builder.RootComponents.Add<App>("app");

#if HOTRELOAD
		builder.UseHotReload(builder.Configuration, ConfigureServices, InitializeServices);
#else
		ConfigureServices(builder.Services, builder.Configuration);
#endif

		await using var app = builder.Build();

#if !HOTRELOAD
		InitializeServices(app.Services);
#endif

		app.MainWindow.SetLogVerbosity(0).SetSize(1920, 900).SetTitle("Piper");

		app.Run();

		return 0;
	}

	/// <summary>
	/// Registers the app services.
	/// Kept separate (and static) so hot reload can call it again, for every new version of this assembly.
	/// </summary>
	public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
	{
		services.AddPiper();
	}

	/// <summary>
	/// Initializes static state that depends on the services.
	/// Kept separate (and static) so hot reload can call it again, for every new version of this assembly (and the libraries).
	/// </summary>
	public static void InitializeServices(IServiceProvider services)
	{
		Log.Factory = services.GetRequiredService<ILoggerFactory>();
	}
}
