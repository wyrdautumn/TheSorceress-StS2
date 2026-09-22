using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary;

namespace TheSorceressMod.TheSorceressModCode.Patches;

[HarmonyPatch (typeof(NCardLibrary), "_Ready")]
public class SubtypeSearchPatch
{
    [HarmonyPostfix]
    public static void SorceressSearchPostfix(NCardLibrary __instance)
    {
        __instance._specialSearchbarKeywords.Add("sorcery", (c => c.Keywords.Contains(SorceressKeywords.Sorcery)));
        __instance._specialSearchbarKeywords.Add("two-weapon", (c => c.Tags.Contains(SorceressKeywords.TwoWeapon)));
    }
}