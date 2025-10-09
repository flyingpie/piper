using Piper.Core.Nodes;
using System.Diagnostics.CodeAnalysis;

namespace Piper.Core.Data;

public class PpNodeInput(PpNodeBase node, string name)
{
	public PpNodeBase Node { get; } = Guard.Against.Null(node);

	public string Name { get; } = Guard.Against.NullOrWhiteSpace(name);

	[MemberNotNullWhen(true, nameof(Output))]
	public bool IsConnected => Output != null;

	public PpNodeOutput? Output { get; set; }

	// public Func<PpTable>? Table { get; set; }
}