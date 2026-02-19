namespace Piper.Core.Data.Modifiers;

public abstract class PpModifier
{
	private readonly List<IPpNodeProperty> _fixedNodeProps;

	protected PpModifier()
	{
		_fixedNodeProps = this.GetModifierProps().ToList();
	}

	public string Id { get; set; } = PpId.Instance.Next();

	public abstract string Name { get; set; }

	// public abstract string ModifierType { get; set; }

	public IEnumerable<IPpNodeProperty> Props => _fixedNodeProps;

	public IPpTable Table { get; } = new PpTable(PpId.Instance.Next());

	public abstract Task ExecuteAsync(IPpTable source, CancellationToken ct = default);
}
