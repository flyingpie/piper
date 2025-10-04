using System.Diagnostics;
using Microsoft.AspNetCore.Components;
using Piper.Core;
using Piper.UI.Services;
using Radzen;
using Radzen.Blazor;
using Piper.Core.Db;

namespace Piper.UI.Components;

public partial class DataViewer : ComponentBase
{
	private RadzenDataGrid<PpRecord> _grid = null!;
	private string _searchTerm;

	public List<PpColumn> Columns = [];

	public List<PpRecord> Records { get; set; } = [];

	public int RecordCount { get; set; }

	public string SearchTerm
	{
		get => _searchTerm;
		set
		{
			_searchTerm = value;
			_grid.Reload();
		}
	}

	protected override async Task OnInitializedAsync()
	{
		SelectedThingyService.Instance.OnChanged(() => InvokeAsync(() => _grid.Reload()));
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			await _grid.Reload();
		}
	}

	private async Task LoadDataAsync(LoadDataArgs args)
	{
		Console.WriteLine("LoadDataAsync");

		var table = SelectedThingyService.Instance.SelectedPort?.Table?.Invoke();
		if (table == null)
		{
			return;
		}

		var sw = Stopwatch.StartNew();

		table.OnChange(_ => InvokeAsync(() => _grid.Reload()));

		Columns = table.Columns;

		// Select
		var sql =
				$"""
				select		*
				from		{table.TableName}
				offset		{args.Skip}
				limit		{args.Top}
				"""
			;

		var sqlCount =
				$"""
				select		count(1)
				from		{table.TableName}
				"""
			;

		RecordCount = (int)await DuckDbPpDb.Instance.CountAsync(sqlCount);
		Records = await DuckDbPpDb.Instance.Query2Async(sql);

		Console.WriteLine($"Data reload took {sw.Elapsed}");
	}
}