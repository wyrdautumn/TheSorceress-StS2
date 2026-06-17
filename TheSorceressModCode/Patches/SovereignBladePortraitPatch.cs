using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;

namespace TheSorceressMod.TheSorceressModCode.Patches;

[HarmonyPatch]
public class SovereignBladePortraitPatch
{
    [HarmonyPatch(typeof(CardModel), nameof(CardModel.PortraitPath), MethodType.Getter)]
    [HarmonyPostfix]
    public static void SovereignBladeSorceressPortraitPostfix(CardModel __instance, ref string __result)
    {
        if (__instance is SovereignBlade && __instance.IsInCombat && __instance.Owner.Character is Character.TheSorceressMod)
        {
            __result = "res://TheSorceressMod/images/card_portraits/sovereign_blade_bird.png";
        }
    }
}