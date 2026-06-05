using HarmonyLib;
using MegaCrit.Sts2.Core.Timeline;

namespace TheSorceressMod.TheSorceressModCode.Patches;

[HarmonyPatch]
public class SorceressEpochPortraitPatch
{
    private const string SorceressPrefix = "THESORCERESSMOD-";
    private const string EpochImageDir = "res://TheSorceressMod/images/epochs/";

    [HarmonyPatch(typeof(EpochModel), nameof(EpochModel.ResolvedPortraitPath), MethodType.Getter)]
    [HarmonyPostfix]
    public static void PlaceholderPortraitPathPostfix(EpochModel __instance, ref string __result)
    {
        if (__instance.Id.StartsWith(SorceressPrefix))
        {
            __result = EpochImageDir + __instance.Id.ToLowerInvariant() + ".png";
        }
    }

    [HarmonyPatch(typeof(EpochModel), nameof(EpochModel.PackedPortraitPath), MethodType.Getter)]
    [HarmonyPostfix]
    public static void SorceressPortraitPathPostfix(EpochModel __instance, ref string __result)
    {
        if (__instance.Id.StartsWith(SorceressPrefix))
        {
            __result = EpochImageDir + __instance.Id.ToLowerInvariant() + "_small.png";
        }
    }
}