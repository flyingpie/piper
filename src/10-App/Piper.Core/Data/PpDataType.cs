using System.Text.Json.Serialization;
using DuckDB.NET.Native;

namespace Piper.Core.Data;

/// <summary>
/// Possible types of record values, maps to <see cref="DuckDBType"/>.<br/>
/// Additionally used to customize the values in serialized graphs.
/// </summary>
public enum PpDataType
{
	/// <summary/>
	[JsonStringEnumMemberName("bool")]
	PpBool,

	/// <summary/>
	[JsonStringEnumMemberName("datetime")]
	PpDateTime,

	/// <summary/>
	[JsonStringEnumMemberName("double")]
	PpDouble,

	/// <summary/>
	[JsonStringEnumMemberName("float")]
	PpFloat,

	/// <summary/>
	[JsonStringEnumMemberName("guid")]
	PpGuid,

	/// <summary/>
	[JsonStringEnumMemberName("i32")]
	PpInt32,

	/// <summary/>
	[JsonStringEnumMemberName("i64")]
	PpInt64,

	/// <summary/>
	// [JsonStringEnumMemberName("json")]
	// PpJson,

	/// <summary/>
	[JsonStringEnumMemberName("string")]
	PpString,

	/// <summary/>
	[JsonStringEnumMemberName("string_array")]
	PpStringArray,
}
