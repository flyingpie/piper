using System.Text.Json.Serialization;

namespace Piper.Core.Data;

public enum PpDataType
{
	[JsonStringEnumMemberName("bool")]
	PpBool,

	[JsonStringEnumMemberName("datetime")]
	PpDateTime,

	[JsonStringEnumMemberName("float")]
	PpFloat,

	[JsonStringEnumMemberName("guid")]
	PpGuid,

	[JsonStringEnumMemberName("i32")]
	PpInt32,

	[JsonStringEnumMemberName("i64")]
	PpInt64,

	[JsonStringEnumMemberName("string")]
	PpString,
}