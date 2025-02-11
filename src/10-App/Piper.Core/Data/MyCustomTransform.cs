namespace Piper.Core.Data;

public class PiperRecord
{
	public List<PiperField> Fields { get; set; } = [];
}

public class PiperField
{
	public object? Value { get; set; }
}

public class PiperDataFrameTransform
{
	public virtual PiperDataFrame Transform(PiperDataFrame frame)
	{
		return frame;
	}

	public virtual PiperRecord Transform(PiperRecord record)
	{
		return record;
	}
}

public class MyCustomTransform
{
	public PiperDataFrame Execute(PiperDataFrame frame)
	{
		foreach (var rec in frame.Records) { }

		return frame;
	}
}
