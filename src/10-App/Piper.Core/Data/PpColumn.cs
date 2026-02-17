using DuckDB.NET.Native;

namespace Piper.Core.Data;

/// <summary>
/// Represents the type- and the name of a single column in a <see cref="IPpTable"/>.
/// </summary>
public class PpColumn(PpDataType dataType, string name)
{
	/// <summary>
	/// Type, maps to <see cref="DuckDBType"/>.
	/// </summary>
	public PpDataType DataType { get; } = dataType;

	/// <summary>
	/// Name of the column, must be SQL friendly (i.e. preferably cased_like_this).
	/// </summary>
	public string Name { get; } = Guard.Against.NullOrWhiteSpace(name);
}
