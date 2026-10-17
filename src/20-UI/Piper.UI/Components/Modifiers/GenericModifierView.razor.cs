using Microsoft.AspNetCore.Components;
using Piper.Core.Modifiers;

namespace Piper.UI.Components.Modifiers;

public partial class GenericModifierView : ComponentBase
{
	[Parameter]
	public PpModifier Modifier { get; set; } = null!;
}
