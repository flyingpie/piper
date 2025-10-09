using Blazor.Diagrams.Core.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Numerics;

namespace Piper.Core.Nodes;

public abstract class PpNodeBase : IPpNode
{
	public virtual string NodeType => GetType().Name;

	public string Name { get; set; }

	public bool IsExecuting { get; set; }

	public ConcurrentQueue<PpLog> Logs { get; set; } = [];

	public Vector2 Position { get; set; }

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

	public abstract NodeModel ToNodeModel();

	protected abstract Task OnExecuteAsync();
}