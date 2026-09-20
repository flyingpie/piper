using System.Linq;
using Microsoft.Extensions.Logging;
using Piper.Core.Logging;

namespace Piper.Core.UnitTest;

public static class TestExtensions
{
	public static void AssertLog(this PpLogs logs, LogLevel level, params string[] messageParts)
	{
		var log = logs.Logs.FirstOrDefault(l => l.Level == level && messageParts.All(mp => l.Message.Contains(mp)));

		if (log == null)
		{
			Assert.Fail(
				$"""
				No log found with level '{level}' and message containing '{string.Join(", ", messageParts)}'.
				These logs were found ({logs.Logs.Count}):
				{string.Join("\n", logs.Logs.Select(l => $"- {l}"))}
				"""
			);
		}
	}
}
