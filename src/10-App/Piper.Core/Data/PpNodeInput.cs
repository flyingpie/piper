using System.Diagnostics.CodeAnalysis;

namespace Piper.Core.Data;

public class PpNodeInput(PpNode node, string name)
{
	public PpNode Node { get; } = Guard.Against.Null(node);

	public string Name { get; } = Guard.Against.NullOrWhiteSpace(name);

	[MemberNotNullWhen(true, nameof(Output))]
	public bool IsConnected => Output != null;

	public PpNodeOutput? Output { get; set; }
}