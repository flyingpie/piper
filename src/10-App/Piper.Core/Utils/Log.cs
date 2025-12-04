using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Piper.Core.Utils;

public static class Log
{
	public static ILoggerFactory Factory { get; set; } = new NullLoggerFactory();

	public static ILogger For(string categoryName) => Factory.CreateLogger(categoryName);

	public static ILogger For(Type type) => Factory.CreateLogger(type);

	public static ILogger For<T>() => Factory.CreateLogger<T>();
}