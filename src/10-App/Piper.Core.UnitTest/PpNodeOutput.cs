using System;

namespace Piper.Core.UnitTest;

public class PpNodeOutput
{
	public PpNodeOutput()
	{
	}

	// public string Name { get; set; }

	public Func<PpDataFrame> DataFrame { get; set; }
}