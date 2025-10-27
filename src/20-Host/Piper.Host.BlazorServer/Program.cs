using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Piper.Core.Utils;
using Piper.UI;
using Piper.UI.Components.Logs;

namespace Piper.Host.BlazorServer;

internal static class Program
{
	public static async Task Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.
		builder.Services.AddPiper();
		builder.Services.AddRazorPages();
		builder.Services.AddServerSideBlazor();

		builder.Logging.AddProvider(BlazorLoggerProvider.Instance);

		var app = builder.Build();

		Log.Factory = app.Services.GetRequiredService<ILoggerFactory>();

		app.UseStaticFiles();

		app.UseRouting();

		app.MapBlazorHub();
		app.MapFallbackToPage("/_Host");

		await app.RunAsync();
	}
}
