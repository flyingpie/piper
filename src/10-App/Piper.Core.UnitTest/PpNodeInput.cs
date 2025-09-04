using System;

namespace Piper.Core.UnitTest;

public class PpNodeInput
{
	// public string Name { get; set; }

	public string? AttributeName { get; set; }

	public Func<PpDataFrame> DataFrame { get; set; }

	public string? Value { get; set; }

	public static implicit operator PpNodeInput(string str) => new() { Value = str };

	public static implicit operator PpNodeInput(PpNodeOutput outp) =>
		new() { DataFrame = () => outp.DataFrame() };

	public static implicit operator PpNodeInput((PpNodeOutput, string) outp) =>
		new() { AttributeName = outp.Item2, DataFrame = () => outp.Item1.DataFrame() };
}