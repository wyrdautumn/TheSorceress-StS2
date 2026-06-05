using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using TheSorceressMod.TheSorceressModCode.Character;

namespace TheSorceressMod.TheSorceressModCode.Patches;

[HarmonyPatch]
public class SorceressEventPatches
{
    private static readonly MethodInfo? EventFinished = AccessTools.Method(
        typeof(EventModel), "SetEventFinished", (Type[])null, (Type[])null);
    
    [HarmonyPatch(typeof(ColorfulPhilosophers))]
    public static class SorceressPhilosophersPatch
    {
        [HarmonyPatch("CardPoolColorOrder", MethodType.Getter)]
        [HarmonyPostfix]
        public static void Postfix(ref IEnumerable<CardPoolModel> __result)
        {
            __result = __result.Append(ModelDb.CardPool<TheSorceressModCardPool>());
        }
    }

    [HarmonyPatch(typeof(ByrdonisNest),"Eat")]
    public static class SorceressByrdonisEatPatch
    {
        [HarmonyPrefix]
        public static void Prefix(ByrdonisNest __instance)
        {
            if (__instance.Owner != null && __instance.Owner.Character is Character.TheSorceressMod)
            {
                EventFinished?.Invoke(__instance, new object?[1] { new LocString("events", "BYRDONIS_NEST.pages.EAT.sorceressDescription") });
            }
        }
    }
    
    [HarmonyPatch(typeof(ByrdonisNest),"Take")]
    public static class SorceressByrdonisTakePatch
    {
        [HarmonyPrefix]
        public static void Prefix(ByrdonisNest __instance)
        {
            if (__instance.Owner != null && __instance.Owner.Character is Character.TheSorceressMod)
            {
                EventFinished?.Invoke(__instance, new object?[1] { new LocString("events", "BYRDONIS_NEST.pages.TAKE.sorceressDescription") });
            }
        }
    }
    
    [HarmonyPatch(typeof(EventModel),"SetInitialEventState")]
    public static class SorceressByrdonisNestInitialPatch
    {
        [HarmonyPostfix]
        public static void Postfix(EventModel __instance)
        {
            if (__instance is ByrdonisNest && __instance.Owner != null && __instance.Owner.Character is Character.TheSorceressMod)
            {
                __instance.Description = new LocString("events", "BYRDONIS_NEST.pages.INITIAL.sorceressDescription");
            }
        }
    }
}