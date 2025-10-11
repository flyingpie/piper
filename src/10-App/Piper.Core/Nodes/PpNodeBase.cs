using Blazor.Diagrams.Core.Models;
using Microsoft.Extensions.Logging;
using Piper.Core.Attributes;
using Piper.Core.Data;
using shortid;
using shortid.Configuration;
using System.Collections.Concurrent;
using System.Reflection;

namespace Piper.Core.Nodes;

public abstract class PpNodeBase : NodeModel, IPpNode
{
	protected PpNodeBase()
	{
	}

	public string NodeId { get; set; } = ShortId.Generate(new GenerationOptions(useNumbers: true, useSpecialCharacters: false));

	public virtual string NodeType => GetType().Name;

	public string Name { get; set; }

	public bool IsExecuting { get; set; }

	public ConcurrentQueue<PpLog> Logs { get; set; } = [];

	// public Vector2 Position { get; set; }

	private List<INodeProperty>? _nodeProps;

	public List<INodeProperty> NodeProps => _nodeProps ??= InitProps().ToList();

	public IEnumerable<MyParam> Params => NodeProps.OfType<MyParam>();

	public IEnumerable<MyPortModel> Ports => NodeProps.OfType<MyPortModel>();

	public void Log(LogLevel level, string message)
		=> Logs.Enqueue(new() { Level = level, Message = message });

	public void Log(string message)
		=> Log(LogLevel.Information, message);

	public void LogWarning(string message)
		=> Log(LogLevel.Warning, message);

	public async Task ExecuteAsync()
	{
		Logs.Clear();

		Log($"Executing node '{GetType().FullName}'");

		var sw = Stopwatch.StartNew();

		IsExecuting = true;

		try
		{
			await OnExecuteAsync();
		}
		catch (Exception ex)
		{
			Log($"Error executing node '{GetType().FullName}': {ex.Message}");
		}

		IsExecuting = false;

		Log($"Executed node '{GetType().FullName}', took {sw.Elapsed}");
	}

	protected abstract Task OnExecuteAsync();

	private IEnumerable<INodeProperty> InitProps()
	{
		var props =
			GetType()
			.GetProperties(BindingFlags.Instance | BindingFlags.Public);

		foreach (var prop in props)
		{
			// Param
			var paramAttr = prop.GetCustomAttribute<PpParamAttribute>();
			if (paramAttr != null)
			{
				yield return new MyParam()
				{
					Name = prop.Name,
					Value = prop.GetValue(this),
					OnSet = v =>
					{
						Console.WriteLine($"ON SET ({v})");
						prop.SetValue(this, v.Value);
					},
				};
				continue;
			}

			// Port
			var inAttr = prop.GetCustomAttribute<PpPortAttribute>();
			if (inAttr != null)
			{
				if (prop.GetValue(this) is PpNodeInput nodeInput)
				{
					var pp = (MyPortModel)AddPort(new MyPortModel(this, PortAlignment.Left));
					pp.PortAttribute = inAttr;
					pp.GetNodeInput = () => nodeInput;
					yield return pp;
				}

				if (prop.GetValue(this) is PpNodeOutput nodeOutput)
				{
					var pp = (MyPortModel)AddPort(new MyPortModel(this, PortAlignment.Right));
					pp.PortAttribute = inAttr;
					pp.GetNodeOutput = () => nodeOutput;
					yield return pp;
				}
			}
		}
	}
}