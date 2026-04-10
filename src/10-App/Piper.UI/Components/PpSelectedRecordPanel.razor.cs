using Microsoft.AspNetCore.Components;
using Piper.Core.Data;
using Piper.UI.Services;

namespace Piper.UI.Components;

public partial class PpSelectedRecordPanel : ComponentBase
{
	public PpRecord? SelectedRecord => SelectedThingyService.Instance.SelectedRecord;

	protected override void OnInitialized()
	{
		SelectedThingyService.Instance.OnSelectedRecord(() => InvokeAsync(StateHasChanged));
	}
}
