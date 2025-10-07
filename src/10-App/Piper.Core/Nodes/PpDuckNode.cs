using Piper.Core.Db;

namespace Piper.Core.Nodes;

public class PpDuckNode : IPpNode
{
	private readonly IPpDb _db = new DuckDbPpDb();

	private PpDataFrame _outLines = new();

	public string NodeType => "SQL";

	public string? Name { get; set; }

	public bool IsExecuting { get; }

	public PpNodeInput In { get; set; } = new();

	public PpNodeOutput OutIncl { get; } = new();

	public string Query { get; set; }

	public async Task ExecuteAsync()
	{
		// // Prep frame
		// _outLines.Records.Clear();
		//
		// // Insert data
		// await _db.LoadAsync(In.DataFrame());
		//
		// // Read data
		// _outLines = await _db.QueryAsync(Query);
		//
		// OutIncl.DataFrame = () => _outLines;
	}
}