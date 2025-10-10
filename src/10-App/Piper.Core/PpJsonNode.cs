using System.Numerics;

namespace Piper.Core;

public class PpJsonNode
{
	public string? Name { get; set; }

	public string? Type { get; set; }

	public PpPos Pos { get; set; }

	public Dictionary<string, PpJsonParam> Params { get; set; } = new();

	public Dictionary<string, PpJsonPort> Ports { get; set; } = new();
}

public class PpJsonParam
{
	public string? StrValue { get; set; }

	public int? IntValue { get; set; }
}

public class PpJsonPort(string node, string port)
{
	public string Node { get; } = Guard.Against.NullOrWhiteSpace(node);

	public string Port { get; } = Guard.Against.NullOrWhiteSpace(port);
}

public class PpPos(double x, double y)
{
	public double X { get; set; } = x;

	public double Y { get; set; } = y;
}