using Npgsql;
using Piper.Core.Attributes;
using Piper.Core.Data;
using static Piper.Core.Data.PpPortDirection;

namespace Piper.Core.Nodes;

public class PpRdbmsNode : PpNode
{
	public PpRdbmsNode()
	{
		OutRecords = new(this, nameof(OutRecords), new(PpTable.GetTableName(this, nameof(OutRecords))));
	}

	public override string Color => "#8a2828";

	public override string Icon => "fa-solid fa-database";

	public override string NodeType => "RDMBS Query";

	public override bool SupportsProgress => false;

	[PpParam("Connection String")]
	public string? InConnectionString { get; set; }

	// [PpParam("Pattern")]
	// public string InPattern { get; set; } = "*";

	// [PpParam("Max Files")]
	// public int MaxFiles { get; set; } = 10_000;

	[PpParam("Query", Hint = PpParamHint.Code)]
	public string Query { get; set; } = "";

	[PpPort(Out, "Records")]
	public PpNodeOutput OutRecords { get; }

	protected override async Task OnExecuteAsync()
	{
		if (string.IsNullOrWhiteSpace(InConnectionString))
		{
			Logs.Warning($"Missing value for param '{nameof(InConnectionString)}'.");
			return;
		}

		// if (string.IsNullOrWhiteSpace(InPattern))
		// {
		// 	Logs.Warning($"Missing value for param '{nameof(InPattern)}'.");
		// 	return;
		// }

		// OutRecords.Table.Columns =
		// [
		// 	new("rec__uuid", PpGuid),
		// 	// new("file", PpDataType.PpJson),
		// 	new("file", PpDataType.PpString),
		// 	// new("file__createdutc", PpDateTime),
		// 	// new("file__dir", PpString),
		// 	// new("file__ext", PpString),
		// 	// new("file__name", PpString),
		// 	// new("file__name_without_ext", PpString),
		// 	// new("file__path", PpString),
		// 	// new("file__size", PpInt32),
		// ];

		await using var conn = new Npgsql.NpgsqlConnection(InConnectionString);
		await conn.OpenAsync();

		await using var comm = conn.CreateCommand();
		comm.CommandText = Query;

		PpDbAppender? appender = null;

		try
		{
			await using var reader = await comm.ExecuteReaderAsync();
			// while (await reader.NextResultAsync())
			while (await reader.ReadAsync())
			{
				if (appender == null)
				{
					var cols = await ReadColumnsAsync(reader);
					OutRecords.Table.Columns.Clear();
					OutRecords.Table.Columns.AddRange(cols);
					await OutRecords.Table.ClearAsync();

					appender = await OutRecords.Table.CreateAppenderAsync();
				}

				// Logs.Info();
				// reader.FieldCount;

				// var record = new PpRecord();
				// for (var i = 0; i < reader.FieldCount; i++)
				// {
				// 	record.Fields[reader.GetName(i)] = new PpField(PpString, reader.GetString(i));
				// 	// OutRecords.Table.Columns.Add(new PpColumn(reader.GetName(i), PpString));
				// }

				var record = await ReadRecordAsync(reader);

				appender.Add(record);

				var dbg = 2;
			}
		}
		finally
		{
			await (appender?.DisposeAsync() ?? ValueTask.CompletedTask);
		}

		await OutRecords.Table.DoneAsync();
	}

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
		{ typeof(string[]),			PpDataType.PpStringArray },
	};
	// csharpier-ignore-end

	public static async Task<ICollection<PpColumn>> ReadColumnsAsync(NpgsqlDataReader reader)
	{
		Guard.Against.Null(reader);

		var cols = new List<PpColumn>();

		for (var i = 0; i < reader.FieldCount; i++)
		{
			var name = reader.GetName(i);
			var type = reader.GetFieldType(i);

			cols.Add(new(name, ToPpDataType(type)));
		}

		return cols;
	}

	public static async Task<PpRecord> ReadRecordAsync(NpgsqlDataReader reader)
	{
		Guard.Against.Null(reader);

		var dict = new Dictionary<string, PpField>();

		for (var i = 0; i < reader.FieldCount; i++)
		{
			var name = reader.GetName(i);
			var type = reader.GetFieldType(i);
			var val2 = reader.GetValue(i);

			dict[name] = new(ToPpDataType(type), val2);
		}

		return new PpRecord() { Fields = dict };
	}

	public static PpDataType ToPpDataType(Type type)
	{
		Guard.Against.Null(type);

		if (_clrTypeToPpType.TryGetValue(type, out var ppType))
		{
			return ppType;
		}

		throw new InvalidOperationException($"Cannot convert type '{type.FullName}' to {nameof(PpDataType)}.");
	}
}
