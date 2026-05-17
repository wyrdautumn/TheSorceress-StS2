using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Cards;

namespace TheSorceressMod.TheSorceressModCode.Patches;

[HarmonyPatch]
public class ReloadOnUpdateVisuals
{
    [HarmonyPatch(typeof(NCard), nameof(NCard.UpdateVisuals))]
    public class NCardReloadOnUpdateVisualsPatch
    {
        [HarmonyPostfix]
        private static void Postfix(NCard __instance)
        {
            if (__instance.IsNodeReady()) __instance.Call("Reload");
        }
    } 
}