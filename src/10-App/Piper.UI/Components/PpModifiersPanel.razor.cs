using Microsoft.AspNetCore.Components;
using Piper.Core.Data.Modifiers;
using Piper.UI.Services;

namespace Piper.UI.Components;

public partial class PpModifiersPanel : ComponentBase
{
	private void AddModifier(string? type)
	{
		Console.WriteLine($"MOD:{type}");
		var mods = SelectedThingyService.Instance.SelectedPort?.Modifiers;
		if (mods == null)
		{
			return;
		}

		switch (type)
		{
			case "PP_MOD_CASING":
				mods.Add(new PpCasingModifier());
				break;

			case "PP_MOD_REMOVE":
				mods.Add(new PpRemoveModifier());
				break;

			case "PP_MOD_REPLACE":
				mods.Add(new PpReplaceModifier());
				break;

			case "PP_MOD_REVERSE":
				mods.Add(new PpReverseModifier());
				break;

			case "PP_MOD_SELECT":
				mods.Add(new PpSelectModifier());
				break;
		}

		// SelectedThingyService.Instance.Changed();
	}

	private void RemoveModifier(PpModifier modifier)
	{
		var mods = SelectedThingyService.Instance.SelectedPort?.Modifiers;
		if (mods == null)
		{
			return;
		}

		mods.Remove(modifier);

		// SelectedThingyService.Instance.Changed();
	}
}
