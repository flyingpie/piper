using System.Threading.Tasks;

namespace Piper.Core.UnitTest;

public interface IPpNode
{
	// IDictionary<string, PpNodeInput> Inputs { get; set; }

	// IDictionary<string, PpNodeOutput> Outputs { get; set; }

	Task ExecuteAsync();
}