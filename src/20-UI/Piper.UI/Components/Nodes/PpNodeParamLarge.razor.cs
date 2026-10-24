using Microsoft.AspNetCore.Components;
using Piper.Core;

namespace Piper.UI.Components.Nodes;

public partial class PpNodeParamLarge : ComponentBase
{
	[EditorRequired]
	[Parameter]
	public PpParam Param { get; set; } = null!;
}
