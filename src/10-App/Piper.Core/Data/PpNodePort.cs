using Piper.Core.Data.Modifiers;

namespace Piper.Core.Data;

/// <summary>
/// Base class for <see cref="PpNodeInput"/> and <see cref="PpNodeOutput"/>.
/// </summary>
public abstract class PpNodePort
{
	protected PpNodePort(PpNode node, string name, IPpTable table)
	{
		Modifiers = new();

		Name = Guard.Against.NullOrWhiteSpace(name);
		Node = Guard.Against.Null(node);
		BaseTable = Guard.Against.Null(table);
	}

	/// <summary>
	/// The name of the port.
	/// </summary>
	public string Name { get; }

	/// <summary>
	/// The node owning this port.
	/// </summary>
	public PpNode Node { get; }

	public PpModifierStack Modifiers { get; }

	public IPpTable BaseTable
	{
		get;
		set
		{
			field = value;
			Modifiers.BaseTable = value;
		}
	}

	public IPpTable Table => Modifiers.Table;
}
