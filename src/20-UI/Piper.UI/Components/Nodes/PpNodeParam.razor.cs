using Microsoft.AspNetCore.Components;
using Piper.UI.Services;

namespace Piper.UI.Components.Nodes;

public partial class PpNodeParam : ComponentBase
{
	[EditorRequired]
	[Parameter]
	public Core.Nodes.PpNodeParam Param { get; set; } = null!;

	[EditorRequired]
	[Parameter]
	public bool IsInNode { get; set; }
}
