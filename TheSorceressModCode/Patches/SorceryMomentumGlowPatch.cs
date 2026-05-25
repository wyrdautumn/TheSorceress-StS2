using BaseLib.Extensions;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using TheSorceressMod.TheSorceressModCode.Powers;

namespace TheSorceressMod.TheSorceressModCode.Patches;

[HarmonyPatch(MethodType.Getter)]
public class SorceryMomentumGlowPatch
{
    [HarmonyPatch(typeof(CardModel), nameof(CardModel.ShouldGlowGold),MethodType.Getter)]
    public class MomentumGlowPatch
    {
        [HarmonyPostfix()]
        private static void Postfix(CardModel __instance, ref bool __result)
        {
            if (__result == false && __instance.CanPlay() && __instance.Keywords.Contains(SorceressKeywords.Sorcery) &&
                __instance.Owner.HasPower<SorcerousMomentumPower>())
            {
                __result = true;
            }
        }
    }
}