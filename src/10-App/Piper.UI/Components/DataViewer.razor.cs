using Microsoft.AspNetCore.Components;
using Piper.Core;
using Piper.Core.Data;
using Piper.Core.Db;
using Piper.UI.Services;
using Radzen;
using Radzen.Blazor;
using System.Diagnostics;

namespace Piper.UI.Components;

public partial class DataViewer : ComponentBase
{
	private RadzenDataGrid<PpRecord> _grid = null!;
	private string _searchTerm;

	public List<PpColumn> Columns = [];

	public IEnumerable<PpRecord> Records { get; set; } = [];

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

	private TextAlign GetAlign(PpColumn col)
	{
		switch (col.PpDataType)
		{
			case PpDataType.PpFloat:
			case PpDataType.PpInt32:
			case PpDataType.PpInt64:
				return TextAlign.Right;

			default:
				return TextAlign.Left;
		}
	}

	private string GetWidth(PpColumn col)
	{
		switch (col.PpDataType)
		{
			case PpDataType.PpDateTime:
				return "12em";

			case PpDataType.PpFloat:
			case PpDataType.PpInt32:
			case PpDataType.PpInt64:
				return "8em";

			case PpDataType.PpGuid:
				return "20em";

			default:
				return "auto";
		}
	}

	private async Task LoadDataAsync(LoadDataArgs args)
	{
		Console.WriteLine("LoadDataAsync");

		// var table = SelectedThingyService.Instance.SelectedPort?.Table?.Invoke();
		var table = SelectedThingyService.Instance.SelectedPort?.GetNodeOutput?.Invoke()?.Table?.Invoke();
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

		RecordCount = (int)await PpDb.Instance.CountAsync(sqlCount);
		// Records = await PpDb.Instance.QueryAsync(sql).ToListAsync();
		Records = await PpDb.Instance.QueryAsync(table, sql).ToListAsync();

		Console.WriteLine($"Data reload took {sw.Elapsed}");
	}
}