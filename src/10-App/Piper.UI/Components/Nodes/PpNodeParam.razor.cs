using Microsoft.AspNetCore.Components;

namespace Piper.UI.Components.Nodes;

public partial class PpNodeParam : ComponentBase
{
	[EditorRequired]
	[Parameter]
	public Core.PpNodeParam Param { get; set; } = null!;

	[EditorRequired]
	[Parameter]
	public bool IsInNode { get; set; }
}