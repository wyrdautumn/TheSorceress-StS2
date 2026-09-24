using HarmonyLib;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace TheSorceressMod.TheSorceressModCode.Patches;

[HarmonyPatch]
public class AgileExhaustGlowPatch
{
    [HarmonyPatch(typeof (NPlayerHand), nameof(NPlayerHand.SelectModeGoldGlowOverride), MethodType.Getter)]
    public static class AgileFromHandPostfix
    {
        [HarmonyPostfix]
        public static void FromHandPostfix(NPlayerHand? __instance, ref Func<CardModel, bool>? __result)
        {
            if (__instance != null && __instance._prefs.Prompt != null && __instance._prefs.Prompt.LocEntryKey == "TO_EXHAUST")
            {
                __result = (c =>
                {
                    if (!c.Keywords.Contains(SorceressKeywords.Subtle))
                        return false;
                    UnplayableReason reason;
                    return c.CanPlay(out reason, out AbstractModel _);
                });
            }
        }
    }
}