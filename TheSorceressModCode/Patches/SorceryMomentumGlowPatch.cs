using BaseLib.Extensions;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using TheSorceressMod.TheSorceressModCode.Powers;

namespace TheSorceressMod.TheSorceressModCode.Patches;

[HarmonyPatch]
public class SorceryMomentumGlowPatch
{
    [HarmonyPatch(typeof(NHandCardHolder), nameof(NHandCardHolder.UpdateCard))]
    public class MomentumGlowPatch
    {
        [HarmonyPostfix]
        private static void Postfix(NHandCardHolder __instance)
        {
            if (__instance.CardNode == null)
                return;
            CardModel? card = __instance.CardNode.Model;
            if (card == null || !card.CanPlay() || card.ShouldGlowGold || card.ShouldGlowRed)
                return;
            if ((card.Owner.HasPower<SorcerousMomentumPower>() && card.Keywords.Contains(SorceressKeywords.Sorcery)) ||
            (card.Owner.HasPower<CunningMomentumPower>() && card.Keywords.Contains(SorceressKeywords.Sleight)))
            {
                __instance.CardNode.CardHighlight.Modulate = new Color(0.742f, 0.43f, 0.996f, 0.98f);
            }
        }
    }
    
    [HarmonyPatch(typeof(NHandCardHolder), nameof(NHandCardHolder.Flash))]
    internal static class SorceressNHandCardHolderFlashHandOutlinePatch
    {
        [HarmonyPostfix]
        public static void Postfix(NHandCardHolder __instance)
        {
            if (__instance.CardNode == null)
                return;
            CardModel? card = __instance.CardNode.Model;
            if (AccessTools.Field(typeof(NHandCardHolder), "_flash")?.GetValue(__instance) is not Control flash ||
                !GodotObject.IsInstanceValid(flash) || card == null || !card.CanPlay() || card.ShouldGlowGold || card.ShouldGlowRed)
                return;
            if ((card.Owner.HasPower<SorcerousMomentumPower>() && card.Keywords.Contains(SorceressKeywords.Sorcery)) ||
                (card.Owner.HasPower<CunningMomentumPower>() && card.Keywords.Contains(SorceressKeywords.Sleight)))
            {
                flash.Modulate = new Color(0.742f, 0.43f, 0.996f, 0.98f);
            }
        }
    }
}