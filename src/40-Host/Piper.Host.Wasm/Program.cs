using System;
using System.Net.Http;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Piper.UI;
using Piper.UI.Components.Logs;
using Piper.Utils;

namespace Piper.Host.Wasm;

internal static class Program
{
	public static async Task Main(string[] args)
	{
		var builder = WebAssemblyHostBuilder.CreateDefault(args);

		builder.RootComponents.Add<App>("#app");
		builder.RootComponents.Add<HeadOutlet>("head::after");

		builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

		builder.Services.AddPiper();
		builder.Logging.AddProvider(BlazorLoggerProvider.Instance);

		var host = builder.Build();

		Log.Factory = host.Services.GetRequiredService<ILoggerFactory>();

		await host.RunAsync();
	}
}
