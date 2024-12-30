using System.Diagnostics.CodeAnalysis;
using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;

namespace Piper.UI.Components;

public partial class PiperNodePort : ComponentBase
{
	[EditorRequired]
	[NotNull]
	[Parameter]
	public PortModel? Port { get; set; }

	[Inject]
	public SelectedThingyService SelectedThingy { get; set; }

	[Parameter]
	public Func<PiperDataFrame>? SelectDataFrame { get; set; }
}