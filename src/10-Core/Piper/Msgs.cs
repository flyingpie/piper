namespace Piper.Core;

public class PpGraphFile
{
	public string Name => System.IO.Path.GetFileName(Path);

	public string Path { get; set; }
}
