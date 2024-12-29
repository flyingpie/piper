using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Piper.UI.Components;

public partial class DataViewer : ComponentBase
{
	[Inject]
	public SelectedThingyService SelectedThingy { get; set; }

	public List<PiperRecord> Records => SelectedThingy?.Node?.Records ?? [];

	// [
	// 	new PiperRecord() { Fields = [new PiperField() { Value = "R1", }] },
	// 	new PiperRecord() { Fields = [new PiperField() { Value = "R2", }] },
	// 	new PiperRecord() { Fields = [new PiperField() { Value = "R3", }] },
	// ];

	protected override async Task OnInitializedAsync()
	{
		SelectedThingy.OnChanged = () =>
		{
			InvokeAsync(() => StateHasChanged());
		};
	}
}
