using Microsoft.AspNetCore.Components;

namespace Piper.UI.Components.Nodes;

public partial class PpNodeParam : ComponentBase
{
	[EditorRequired]
	[Parameter]
	public MyParam Param { get; set; } = null!;
}