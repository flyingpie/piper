using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Pipelines;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using MoreLinq;

namespace Piper.Core.UnitTest;

public class PpDataFrame
{
	public List<PpRecord> Records { get; set; } = [];

	public IEnumerable<string> FieldNames =>
		Records
			.SelectMany(r => r.Fields.Keys)
			.Distinct();

	public override string ToString() => $"{Records.Count} records";
}

public class PpRecord
{
	public IDictionary<string, PpField> Fields { get; set; } =
		new Dictionary<string, PpField>(StringComparer.OrdinalIgnoreCase);

	public override string ToString() => string.Join(", ", Fields);
}

public class PpField
{
	public PpField()
	{
	}

	public PpField(Guid? valueAsGuid)
	{
		ValueAsGuid = valueAsGuid;
	}

	public PpField(int? valueAsInt)
	{
		ValueAsInt = valueAsInt;
	}

	public PpField(string? valueAsString)
	{
		ValueAsString = valueAsString;
	}

	// public string Name { get; set; }

	public Guid? ValueAsGuid { get; set; }

	public int? ValueAsInt { get; set; }

	public string? ValueAsString { get; set; }

	public object? Value => ValueAsInt?.ToString() ?? ValueAsString;

	public static implicit operator PpField(int? valueAsInt) => new(valueAsInt);

	public static implicit operator PpField(string str) => new(str);

	public override string ToString() => Value?.ToString() ?? "(empty)";
}

public class PpRegexNode : IPpNode
{
	private PpDataFrame _match = new();
	private PpDataFrame _noMatch = new();

	public PpNodeInput InPattern { get; set; }

	public PpNodeInput OutMatch { get; set; } = new();

	public PpNodeOutput OutNoMatch { get; } = new();

	public async Task ExecuteAsync()
	{
		_match.Records.Clear();
		_noMatch.Records.Clear();

		var regex = new Regex(InPattern.Value, RegexOptions.Compiled);

		var resMatch = new List<PpRecord>();

		var resNoMatch = new List<PpRecord>();

		foreach (var rec in OutMatch.DataFrame().Records)
		{
			var f = rec.Fields.ToDictionary();

			var match = regex.Match(f[OutMatch.AttributeName].ValueAsString);
			if (match.Success)
			{
				foreach (var grp in match.Groups.OfType<Group>())
				{
					if (!char.IsAsciiLetter(grp.Name[0]))
					{
						continue;
					}

					f[grp.Name] = new(grp.Value);
				}

				var rec2 = new PpRecord()
				{
					Fields = f,
				};

				resMatch.Add(rec2);
			}
			else
			{
				var rec2 = new PpRecord()
				{
					Fields = f,
				};
				resNoMatch.Add(rec2);
			}
		}

		_match = new()
		{
			Records = resMatch,
		};

		_noMatch = new()
		{
			Records = resNoMatch,
		};

		OutMatch.DataFrame = () => _match;
		OutNoMatch.DataFrame = () => _noMatch;
	}
}