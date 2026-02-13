using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Runtime.Loader;
using System.Runtime.Serialization;
using System.Threading;
using Piper.Core.Db;

namespace Piper.Core.Data;

public class PpTable(string tableName, ICollection<PpColumn> columns)
{
	private Action<PpTable> _onChange = _ => { };

	public PpTable(string tableName)
		: this(tableName, []) { }

	public long Count { get; set; }

	public string TableName { get; } = Guard.Against.NullOrWhiteSpace(tableName);

	public List<PpColumn> Columns { get; set; } = Guard.Against.Null(columns).ToList();

	public List<PpModifier> Modifiers { get; set; } = [];

	public static string GetTableName(PpNode node, string propName)
	{
		return $"{node.GetType().Name}_{node.NodeId}_{propName}";
	}

	public void Changed()
	{
		_onChange(this);
	}

	public void OnChange(Action<PpTable> onChange)
	{
		Guard.Against.Null(onChange);

		_onChange = onChange;
	}

	public async Task ClearAsync(CancellationToken ct = default)
	{
		// await PpDb.Instance.CreateTableAsync(this);
		await PpDb.Instance.CreateTableAsync(this);

		_onChange?.Invoke(this);
	}

	public async Task InitAsync()
	{
		await PpDb.Instance.InitTableAsync(this);

		await DoneAsync();
	}

	// public async Task AddAsync(PpRecord record)
	// {
	// 	Guard.Against.Null(record);
	//
	// 	await AddAsync([record]);
	// }

	// public async Task AddAsync(IEnumerable<PpRecord> records)
	// {
	// 	ArgumentNullException.ThrowIfNull(records);
	//
	// 	var db = PpDb.Instance;
	//
	// 	// await db.InsertDataAsync(TableName, records);
	// 	await db.V_InsertDataAsync(this, records);
	// }

	public Task<PpDbAppender> CreateAppenderAsync()
	{
		return PpDb.Instance.CreateAppenderAsync(this);
	}

	public async Task DoneAsync()
	{
		Count = await CountAsync();

		_onChange?.Invoke(this);
	}

	public async Task<long> CountAsync()
	{
		return await PpDb.Instance.CountAsync($"select count(1) from \"{TableName}\"");
	}

	public IAsyncEnumerable<PpRecord> QueryAllAsync()
	{
		// return PpDb.Instance.QueryAsync($"select * from {TableName}");

		return QueryAsync($"select * from {TableName}");

		// await foreach (var rec in PpDb.Instance.QueryAsync(this, "select * from $table"))
		// {
		// 	var x = rec;
		//
		// 	foreach (var mod in Modifiers)
		// 	{
		// 		x = await mod.ExecuteAsync(x);
		// 	}
		//
		// 	yield return x;
		// }
	}

	public async IAsyncEnumerable<PpRecord> QueryAsync(string sql)
	{
		// await foreach (var rec in PpDb.Instance.QueryAsync(this, "select * from $table"))
		await foreach (var rec in PpDb.Instance.QueryAsync(this, sql))
		{
			var x = rec;

			foreach (var mod in Modifiers)
			{
				x = await mod.ExecuteAsync(x);
			}

			yield return x;
		}
	}
}

public abstract class PpModifier
{
	public abstract string Name { get; set; }

	public abstract Task<PpRecord> ExecuteAsync(PpRecord record);
}

public class PpCasingModifier : PpModifier
{
	public override string Name { get; set; } = "Casing";

	public string FieldName { get; set; } = "rec__uuid";

	public override Task<PpRecord> ExecuteAsync(PpRecord record)
	{
		if (FieldName == null)
		{
			return Task.FromResult(record);
		}

		if (record.Fields.TryGetValue(FieldName, out var field))
		{
			field.Value = field.Value?.ToString()?.ToUpperInvariant();
		}

		// record.Fields["new_field"] = "A string";

		return Task.FromResult(record);
	}
}

[DataContract(Name = "Reverse", Namespace = "PpModifier")]
public class PpReverseModifier : PpModifier
{
	public override string Name { get; set; } = "Reverse";

	public string FieldName { get; set; } = "rec__uuid";

	public override Task<PpRecord> ExecuteAsync(PpRecord record)
	{
		// record.Fields["new_field"] = "A string";

		if (FieldName == null)
		{
			return Task.FromResult(record);
		}

		record.Fields["new_field"] = string.Empty;
		if (record.Fields.TryGetValue(FieldName, out var field))
		{
			record.Fields["new_field"] = new string(field.Value?.ToString()?.Reverse()?.ToArray() ?? []);
		}

		return Task.FromResult(record);
	}
}

public class PluginLoadContext : AssemblyLoadContext
{
	// public readonly AssemblyDependencyResolver _resolver;

	public PluginLoadContext()
	{
		// _resolver = new();
	}

	protected override Assembly? Load(AssemblyName assemblyName)
	{
		return base.Load(assemblyName);
	}

	protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
	{
		return base.LoadUnmanagedDll(unmanagedDllName);
	}
}
