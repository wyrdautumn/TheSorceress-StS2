using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace TheSorceressMod.TheSorceressModCode.Patches;

[HarmonyPatch (typeof(TalkCmd), "GetDuration")]
public class TalkCmdDurationPostfix
{
    [HarmonyPostfix]
    public static void ExtraVeryLongDurationPostfix(VfxDuration duration, ref double __result)
    {
        if (duration == SorceressKeywords.ExtraVeryLong)
            __result = 5.0;
    }
}