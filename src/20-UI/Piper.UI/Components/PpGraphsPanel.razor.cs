using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using Blazor.Diagrams;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Piper.Core;
using Piper.Core.Serialization;

namespace Piper.UI.Components;

public partial class PpGraphsPanel : ComponentBase
{
	private const string ServerGraphDir = "/home/marco/workspace/flyingpie/piper_1/graphs";
	private const string BrowserGraphDir = "graphs";

	[Inject]
	private BlazorDiagram? Diagram { get; set; }

	[Inject]
	private IServiceProvider Services { get; set; } = null!;

	public List<PpGraphFile> Graphs { get; set; } = [];

	protected override async Task OnInitializedAsync()
	{
		Graphs = OperatingSystem.IsBrowser() ? await LoadBrowserGraphsAsync() : LoadServerGraphs();
	}

	private static List<PpGraphFile> LoadServerGraphs()
	{
		return [.. Directory.GetFiles(ServerGraphDir, "*.json").Select(p => new PpGraphFile() { Path = p })];
	}

	private async Task<List<PpGraphFile>> LoadBrowserGraphsAsync()
	{
		// Browsers can't enumerate the local file system; the host serves a manifest of available graphs instead.
		var http = Services.GetService<HttpClient>();

		if (http is null)
		{
			return [];
		}

		try
		{
			var names = await http.GetFromJsonAsync<List<string>>($"{BrowserGraphDir}/index.json");

			return [.. (names ?? []).Select(name => new PpGraphFile() { Path = $"{BrowserGraphDir}/{name}" })];
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Failed to load graph index: {ex.Message}");

			return [];
		}
	}

	private async Task LoadGraphFileAsync(PpGraphFile file)
	{
		var json = OperatingSystem.IsBrowser() ? await ReadBrowserGraphAsync(file) : await File.ReadAllTextAsync(file.Path);

		if (json is null)
		{
			return;
		}

		var graph = PpNodeSerializer.DeserializeGraph(json);

		Diagram!.LoadGraph(graph);
		Diagram.ZoomToFit(25);
	}

	private async Task SaveGraphFileAsync(PpGraphFile file)
	{
		var graph = PpNodeSerializer.SerializeGraphJson(Diagram!.GetGraph());

		if (OperatingSystem.IsBrowser())
		{
			// Graphs are served read-only to the browser, so there's nothing to write back to.
			Console.WriteLine($"Saving graphs is not supported in the browser ('{file.Path}').");

			return;
		}

		await File.WriteAllTextAsync(file.Path, graph);
	}

	private async Task<string?> ReadBrowserGraphAsync(PpGraphFile file)
	{
		var http = Services.GetService<HttpClient>();

		if (http is null)
		{
			return null;
		}

		return await http.GetStringAsync(file.Path);
	}
}
