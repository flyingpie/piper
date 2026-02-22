namespace Piper.Core;

public class Msgs
{
	public static Msgs Instance { get; } = new();

	// public void OnMessage
}

public class LoadGraphMsg
{
	public PpGraphFile File { get; set; }
}

public class SaveGraphMsg
{
	public PpGraphFile File { get; set; }
}

// public class SelectTabMsg
// {
// 	public string TabName
// }

public class PpGraphFile
{
	public string Name => System.IO.Path.GetFileName(Path);

	public string Path { get; set; }
}
