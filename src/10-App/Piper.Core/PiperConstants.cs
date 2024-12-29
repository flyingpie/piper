using System;

namespace Piper.Core;

public static class PiperConstants
{
	public static string AppVersion { get; }
		= typeof(PiperConstants).Assembly.GetName().Version?.ToString() ?? "<unknown>";

	public static Uri DocumentationUrl { get; }
		= new("https://wtq.flyingpie.nl");

	public static Uri GitHubUrl { get; }
		= new("https://www.github.com/flyingpie/windows-terminal-quake");
}