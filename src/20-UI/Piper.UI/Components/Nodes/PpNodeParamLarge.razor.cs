using Microsoft.AspNetCore.Components;

namespace Piper.UI.Components.Nodes;

public partial class PpNodeParamLarge : ComponentBase
{
	[EditorRequired]
	[Parameter]
	public Core.Nodes.PpNodeParam Param { get; set; } = null!;
}
