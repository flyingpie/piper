using Piper.Core.Logging;

namespace Piper.Core.Data.Modifiers;

public abstract class PpModifier
{
	private readonly List<IPpNodeProperty> _fixedNodeProps;

	protected PpModifier()
	{
		_fixedNodeProps = this.GetModifierProps().ToList();
	}

	public string Id { get; set; } = PpId.Instance.NextMod();

	public PpLogs Logs { get; } = new();

	public abstract string Name { get; set; }

	public IEnumerable<IPpNodeProperty> Props => _fixedNodeProps;

	public IPpTable Table { get; } = new PpTable(PpId.Instance.NextTable());

	public abstract Task ExecuteAsync(IPpTable source, CancellationToken ct = default);
}
