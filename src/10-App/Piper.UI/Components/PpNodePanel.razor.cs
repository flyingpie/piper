using Microsoft.AspNetCore.Components;
using Piper.Core;

namespace Piper.UI.Components;

public partial class PpNodePanel : ComponentBase
{
	[Parameter]
	public PpNode? SelectedNode { get; set; }
}
