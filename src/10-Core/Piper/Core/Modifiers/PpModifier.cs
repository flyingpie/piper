using Piper.Core.Logging;
using Piper.Core.Nodes;

namespace Piper.Core.Modifiers;

/// <summary>
/// A "modifier" takes in a table, manipulates its records somehow, and returns the result as a new table.<br/>
/// A modifier may actually create a new table in the database, or just build a view on top of the input table.<br/>
/// By actually persisting the transform, we can easily do things afterward, like pagination.
/// </summary>
public abstract class PpModifier
{
	protected PpModifier()
	{
		Props = this.GetModifierProps();
	}

	/// <summary>
	/// Each modifier gets its own id, useful for uniquely referring to it, and serialization.
	/// </summary>
	public string Id { get; set; } = PpId.Instance.NextMod();

	/// <summary>
	/// Local logs.
	/// </summary>
	public PpLogs Logs { get; } = new();

	/// <summary>
	/// Descriptive name for display in the UI.
	/// </summary>
	public abstract string Name { get; set; }

	/// <summary>
	/// Holds properties that the user may interact with.
	/// </summary>
	public IReadOnlyCollection<IPpNodeProperty> Props { get; }

	/// <summary>
	/// Table used by the modifier to store its manipulated data in (i.e., the output table).
	/// </summary>
	public IPpTable Table { get; } = new PpTable();

	/// <summary>
	/// Called to ask the modifier to take in the records from <paramref name="source"/>,
	/// and store the augmented results in <see cref="Table"/>.
	/// </summary>
	public abstract Task ExecuteAsync(IPpTable source, CancellationToken ct = default);
}
