using Microsoft.Extensions.Logging;

namespace Piper.Core.Utils;

public static class Log
{
	public static ILoggerFactory Factory { get; set; }

	public static ILogger For(string categoryName) => Factory.CreateLogger(categoryName);

	public static ILogger For(Type type) => Factory.CreateLogger(type);

	public static ILogger For<T>() => Factory.CreateLogger<T>();
}