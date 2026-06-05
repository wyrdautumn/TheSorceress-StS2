using HarmonyLib;
using MegaCrit.Sts2.Core.Timeline;
using MegaCrit.Sts2.Core.Timeline.Epochs;
using TheSorceressMod.TheSorceressModCode.Epochs;

namespace TheSorceressMod.TheSorceressModCode.Patches;

[HarmonyPatch(typeof(NeowEpoch), "GetTimelineExpansion")]
public static class SorceressNeowExpansionPatch
{
    [HarmonyPostfix]
    public static void Postfix(ref EpochModel[] __result)
    {
        var list = new List<EpochModel>(__result);
        
        var epoch = (EpochModel)Activator.CreateInstance(typeof (Sorceress4Epoch))!;
        if (!list.Any(e => e.Id == epoch.Id))
            list.Add(epoch);

        __result = list.ToArray();
    }
}