using Blazor.Diagrams;

namespace Piper.Core;

public class PiperGraph : BlazorDiagram
{
	public string Save()
	{
		return null;
	}
}

public class PgNode
{
	public string? Name { get; set; }
}

public class PgEdge { }
