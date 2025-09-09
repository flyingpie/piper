using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Piper.UI;

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

		var app = builder.Build();

		app.UseStaticFiles();

		app.UseRouting();

		app.MapBlazorHub();
		app.MapFallbackToPage("/_Host");

		await app.RunAsync();
	}
}
