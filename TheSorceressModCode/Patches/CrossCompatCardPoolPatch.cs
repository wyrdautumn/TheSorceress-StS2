using BaseLib.Abstracts;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using TheSorceressMod.TheSorceressModCode.Cards.CrossMod;
using TheSorceressMod.TheSorceressModCode.Character;

namespace TheSorceressMod.TheSorceressModCode.Patches;

[HarmonyPatch]
public class CrossCompatCardPoolPatch
{
    [HarmonyPatch(typeof(CardPoolModel), "AllCards", MethodType.Getter)]
    public static class SorceressPoolPostfix
    {
        [HarmonyPostfix]
        public static void SorceressCardPoolPostfix(ref IEnumerable<CardModel> __result, CardPoolModel __instance)
        {
            if (__instance is not TheSorceressModCardPool && __instance is not TokenCardPool && __instance is not EventCardPool)
                return;
            
            List<CardModel> list = __result.ToList();
            
            var mysteriousFlashlightType =
                AccessTools.TypeByName("TheHeroExpansion.TheHeroExpansionCode.Extensions.CustomMysteriousFlashlightExtension");
            if (mysteriousFlashlightType == null)
            {
                list.RemoveAll(c => c is TheSorceressModHeroExpansionCard);
            }
        
            var ancientsAwakenedType =
                AccessTools.TypeByName("AncientsAwakened.AncientsAwakenedCode.Pools.Mithrix.PerfectedPool");
            if (ancientsAwakenedType == null)
            {
                list.RemoveAll(c => c is TheSorceressModAncientsAwakenedCard);
            }
        
        
            var deckboxType = AccessTools.TypeByName("MoreNeow.MoreNeowCode.Relics.Complex.UnfamiliarDeckbox");
            if (deckboxType == null)
            {
                list.RemoveAll(c => c is TheSorceressModMoreNeowCard);
            }

            __result = list;
        }
    }
}