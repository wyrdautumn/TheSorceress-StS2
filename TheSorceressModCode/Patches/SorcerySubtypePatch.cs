using System.Reflection.Emit;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;

namespace TheSorceressMod.TheSorceressModCode.Patches;

[HarmonyPatch(typeof(NCard), "UpdateTypePlaque")]
public class SorcerySubtypePatch
{
    [HarmonyTranspiler]
    static List<CodeInstruction> AddSorcerySubtype(IEnumerable<CodeInstruction> instructions)
    {
        var codeMatcher = new CodeMatcher(instructions);

        codeMatcher
            .MatchStartForward(
                CodeMatch.Calls(typeof(CardTypeExtensions).Method(nameof(CardTypeExtensions.ToLocString))),
                CodeMatch.Calls(typeof(LocString).Method(nameof(LocString.GetFormattedText))),
                CodeMatch.Calls(typeof(MegaLabel).Method(nameof(MegaLabel.SetTextAutoSize)))
            )
            .InsertAfterAndAdvance(
                CodeInstruction.LoadArgument(0),
                new CodeInstruction(OpCodes.Call, typeof(NCard).PropertyGetter(nameof(NCard.Model))),
                CodeInstruction.Call(typeof(SorcerySubtypePatch), nameof(TryModifyPlaqueText))
            );

        return codeMatcher.Instructions();
    }

    private static LocString TryModifyPlaqueText(LocString originalPlaqueText, CardModel card)
    {
        if (card.Keywords.Contains(SorceressKeywords.Sorcery))
        {
            LocString sorceryString;
            sorceryString = new LocString("gameplay_ui", "THESORCERESSMOD-CARD_TYPE.SORCERY");
            sorceryString.Add("Type",originalPlaqueText);
            return sorceryString;
        }
        else
            return originalPlaqueText;
    }
}

[HarmonyPatch(typeof(NCard), "UpdateTypePlaque")]
public class SubtypeColorPatch
{
    [HarmonyPostfix]
    static void ChangePlaqueColor(NCard __instance)
    {
        if (__instance.Model == null)
            return;
        if (__instance.Model.Keywords.Contains(SorceressKeywords.Sorcery))
            __instance._typePlaque.Material = PreloadManager.Cache.GetMaterial("res://TheSorceressMod/images/shaders/card_banner_sorcery_mat.tres");
        if (!__instance.Model.Keywords.Contains(SorceressKeywords.Sorcery))
            if (__instance._typePlaque.Material != __instance.Model.BannerMaterial)
                __instance._typePlaque.Material = __instance.Model.BannerMaterial;
    }
}
