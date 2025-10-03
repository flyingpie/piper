using Dapper;
using DuckDB.NET.Data;
using Microsoft.AspNetCore.Components;
using Piper.Core;
using Piper.UI.Services;
using Radzen;
using Radzen.Blazor;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Piper.UI.Components;

public partial class DataViewer : ComponentBase
{
	private RadzenDataGrid<PpRecord> _grid = null!;

	public class Stuff
	{
		public string? Val1 { get; set; }
	}

	// [Inject]
	// public SelectedThingyService? SelectedThingy { get; set; }

	public IEnumerable<string> FieldNames = [];

	public List<PpRecord> Records { get; set; } = [];

	public int RecordCount { get; set; }

	public List<PpRecord> RecordsSub { get; set; } = [];

	// {
	// 	get
	// 	{
	// 		//
	// 		return SelectedThingy
	// 				?.Node
	// 				?.Records
	// 			// ?.Select(r => new Stuff()
	// 			// {
	// 			// 	Val1 = "sup11",
	// 			// })
	// 			?? [];
	// 	}
	// }

	// public int ReptileCount { get; set; }

	// public ICollection<Reptile> Reptiles { get; set; } = [];

	// [
	// 	new PiperRecord() { Fields = [new PiperField() { Value = "R1", }] },
	// 	new PiperRecord() { Fields = [new PiperField() { Value = "R2", }] },
	// 	new PiperRecord() { Fields = [new PiperField() { Value = "R3", }] },
	// ];

	protected override async Task OnInitializedAsync()
	{
		SelectedThingyService.Instance.OnChanged(() => InvokeAsync(() =>
		{
			FieldNames = SelectedThingyService.Instance?.Node?.FieldNames?.ToList() ?? [];

			Records = SelectedThingyService.Instance
					?.Node
					?.Records
				// ?.Select(rec => new Reptile() { ganzh = rec.Fields.Values.FirstOrDefault()?.ValueAsString })
				// ?.ToList()
				?? [];

			RecordCount = Records.Count;

			// StateHasChanged();
			_grid.Reload();
		}));
	}

	private string _searchTerm;

	public string SearchTerm
	{
		get => _searchTerm;
		set
		{
			_searchTerm = value;
			_grid.Reload();
		}
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
		RecordsSub = Records
			.Where(rec => string.IsNullOrWhiteSpace(_searchTerm) || rec.Fields.Any(f => f.Value?.ValueAsString?.Contains(_searchTerm, StringComparison.OrdinalIgnoreCase) ?? false))
			.Skip(args.Skip ?? 0)
			.Take(args.Top ?? 50)
			.ToList();

		// using var db = await CreateConnectionAsync();
		//
		// var sb = new StringBuilder();
		//
		// // Select
		//
		// // Filter
		// var filter = args.Filters.FirstOrDefault();
		// if (filter != null)
		// {
		// 	switch (filter.FilterOperator)
		// 	{
		// 		case FilterOperator.Contains:
		// 		default:
		// 			sb.AppendLine($"where {filter.Property} ilike '%{filter.FilterValue}%'");
		// 			break;
		// 	}
		// }
		//
		// // Order
		// var sort = args.Sorts.FirstOrDefault();
		// if (sort != null)
		// {
		// 	sb.AppendLine($"ORDER BY {sort.Property} {(sort.SortOrder == SortOrder.Ascending ? "asc" : "desc")}");
		// }
		//
		// sb.AppendLine($"offset {args.Skip} limit {args.Top}");
		//
		// var sb2 = new StringBuilder();
		// sb2.AppendLine("select count(1) from reptiles");
		// sb2.AppendLine(sb.ToString());
		//
		// ReptileCount = await db.ExecuteScalarAsync<int>(sb2.ToString());
		//
		// var sb1 = new StringBuilder();
		// sb1.AppendLine("select * from reptiles");
		// sb1.AppendLine(sb.ToString());
		// var q = sb1.ToString();
		// Console.WriteLine("=== QUERY ===");
		// Console.WriteLine(q);
		// Console.WriteLine("=== /QUERY ===");
		//
		// var res = await db.QueryAsync<Reptile>(q);
		// Reptiles = res.ToList();
	}

	private async Task<DuckDBConnection> CreateConnectionAsync()
	{
		var db = new DuckDBConnection("data source=/home/marco/Downloads/reptiles.db");
		await db.OpenAsync();

		return db;
	}
}

public class Reptile
{
	[Column("ganzh")]
	public string? ganzh { get; set; }
}