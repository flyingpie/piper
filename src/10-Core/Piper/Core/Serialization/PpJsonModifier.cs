namespace Piper.Core.Serialization;

public class PpJsonModifier
{
	public PpJsonNodeTypeAndName Id { get; set; }

	public Dictionary<string, string> Params { get; set; } = [];
}
