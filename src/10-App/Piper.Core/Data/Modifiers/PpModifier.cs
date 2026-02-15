using shortid;
using shortid.Configuration;

namespace Piper.Core.Data.Modifiers;

public abstract class PpModifier
{
	public abstract string Name { get; set; }

	public IPpTable Table { get; } = new PpTable(ShortId.Generate(new GenerationOptions(useNumbers: true, useSpecialCharacters: false)));

	public abstract Task ExecuteAsync(IPpTable source, CancellationToken ct = default);
}
