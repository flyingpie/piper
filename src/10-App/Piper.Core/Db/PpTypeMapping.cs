using DuckDB.NET.Native;
using Piper.Core.Data;

namespace Piper.Core.Db;

public static class PpTypeMapping
{
	// csharpier-ignore-start
	private static readonly Dictionary<Type, PpDataType> _clrTypeToPpType = new()
	{
		{ typeof(bool),				PpDataType.PpBool },
		{ typeof(DateTime),			PpDataType.PpDateTime },
		{ typeof(double),			PpDataType.PpDouble },
		{ typeof(float),			PpDataType.PpFloat },
		{ typeof(Guid),				PpDataType.PpGuid },
		{ typeof(int),				PpDataType.PpInt32 },
		{ typeof(long),				PpDataType.PpInt64 },
		// { typeof(object),			PpDataType.PpJson },
		{ typeof(string),			PpDataType.PpString },
		{ typeof(List<string>),		PpDataType.PpStringArray },
	};

	private static readonly Dictionary<DuckDBType, PpDataType> _duckTypeToPpType = new()
	{
		{ DuckDBType.Boolean,		PpDataType.PpBool },
		{ DuckDBType.Timestamp,		PpDataType.PpDateTime },
		{ DuckDBType.Double,		PpDataType.PpDouble },
		{ DuckDBType.Float,			PpDataType.PpFloat },
		{ DuckDBType.Uuid,			PpDataType.PpGuid },
		{ DuckDBType.Integer,		PpDataType.PpInt32 },
		// { DuckDBType.Json,		PpDataType.PpInt32 },
		{ DuckDBType.BigInt,		PpDataType.PpInt64 },
		{ DuckDBType.Varchar,		PpDataType.PpString },
		{ DuckDBType.Array,			PpDataType.PpStringArray },
	};
	// csharpier-ignore-end

	public static string ToDuckDbColumnSql(this PpColumn column)
	{
		var name = $"\"{column.Name.Replace(" ", "_")}\"";

		switch (column.DataType)
		{
			case PpDataType.PpBool:
				return $"{name} BOOLEAN NULL";

			case PpDataType.PpDateTime:
				return $"{name} TIMESTAMP NULL";

			case PpDataType.PpDouble:
				return $"{name} DOUBLE NULL";

			case PpDataType.PpFloat:
				return $"{name} REAL NULL";

			case PpDataType.PpGuid:
				return $"{name} UUID NULL";

			case PpDataType.PpInt32:
				return $"{name} INTEGER NULL";

			case PpDataType.PpInt64:
				return $"{name} BIGINT NULL";

			// case PpDataType.PpJson:
			// 	return $"{name} JSON NULL";

			case PpDataType.PpString:
				return $"{name} TEXT NULL";

			case PpDataType.PpStringArray:
				return $"{name} TEXT[] NULL";

			default:
				throw new InvalidOperationException($"Unsupported column '{column.DataType}'");
		}
	}

	public static PpDataType ToPpDataType(this DuckDBType type)
	{
		Guard.Against.Null(type);

		if (_duckTypeToPpType.TryGetValue(type, out var ppType))
		{
			return ppType;
		}

		throw new InvalidOperationException($"Cannot convert DuckDB type '{type}' to {nameof(PpDataType)}.");
	}

	public static PpDataType ToPpDataType(this Type type)
	{
		Guard.Against.Null(type);

		if (_clrTypeToPpType.TryGetValue(type, out var ppType))
		{
			return ppType;
		}

		throw new InvalidOperationException($"Cannot convert type '{type.FullName}' to {nameof(PpDataType)}.");
	}
}
