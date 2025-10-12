using Microsoft.AspNetCore.Components;
using Piper.Core;

namespace Piper.UI.Components.Nodes;

public partial class PpNodeParam : ComponentBase
{
	[EditorRequired]
	[Parameter]
	public Core.PpNodeParam Param { get; set; } = null!;
}