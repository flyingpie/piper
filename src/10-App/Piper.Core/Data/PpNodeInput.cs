using System.Diagnostics.CodeAnalysis;

namespace Piper.Core.Data;

public class PpNodeInput
{
	[MemberNotNullWhen(true, nameof(Table))]
	public bool IsConnected => Table != null;

	public Func<PpTable>? Table { get; set; }
}