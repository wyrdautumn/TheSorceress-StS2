using HarmonyLib;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using TheSorceressMod.TheSorceressModCode.Character;

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

        if (!TheSorceressModConfig.RestorePriestess && __instance is TheHighPriestess)
            __result = "res://TheSorceressMod/images/card_portraits/the_sorceress_tarot.png";
    }
    
    [HarmonyPatch(typeof(CardModel), nameof(CardModel.PortraitPngPath), MethodType.Getter)]
    [HarmonyPostfix]
    public static void SovereignBladeSorceressBigPortraitPostfix(CardModel __instance, ref string __result)
    {
        if (__instance is SovereignBlade && __instance.IsInCombat && __instance.Owner.Character is Character.TheSorceressMod)
        {
            __result = "res://TheSorceressMod/images/card_portraits/big/sovereign_blade_bird.png";
        }
        
        if (!TheSorceressModConfig.RestorePriestess && __instance is TheHighPriestess)
            __result = "res://TheSorceressMod/images/card_portraits/big/the_sorceress_tarot.png";
    }

    [HarmonyPatch(typeof(CardModel), nameof(CardModel.TitleLocString), MethodType.Getter)]
    [HarmonyPostfix]
    public static void TheSorceressTarotPostfix(CardModel __instance, ref LocString __result)
    {
        if (!TheSorceressModConfig.RestorePriestess && __instance is TheHighPriestess)
            __result = new LocString("cards", "THESORCERESSMOD-HIGH_PRIESTESS.title");
    }
}