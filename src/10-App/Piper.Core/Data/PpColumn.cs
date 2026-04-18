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

	public bool IsVisible { get; set; } = true;

	public static implicit operator PpColumn((PpDataType Type, string Name) f) => new(f.Type, f.Name);

	// public static implicit operator PpColumn((string Name) f) => new(f.Name, PpDataType.PpString, f.Value);
	//
	// public static implicit operator PpColumn((string Name, bool? Value) f) => new(f.Name, PpDataType.PpBool, f.Value);
}
