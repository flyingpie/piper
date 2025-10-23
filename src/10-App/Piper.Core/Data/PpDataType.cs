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

	[JsonStringEnumMemberName("int")]
	PpInt,

	[JsonStringEnumMemberName("string")]
	PpString,
}