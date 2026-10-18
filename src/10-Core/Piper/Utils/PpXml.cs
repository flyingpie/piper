namespace Piper.Utils;

public static class PpXml
{
	public static bool LooksLikeXml(string str) => str?.StartsWith('<') ?? false;
}
