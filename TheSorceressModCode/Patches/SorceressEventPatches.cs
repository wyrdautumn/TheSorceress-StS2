using System.Reflection;
using System.Reflection.Emit;
using BaseLib.Utils.Patching;
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

    [HarmonyPatch(typeof(ByrdonisNest), "Eat", MethodType.Async)]
    public class SorceressNestEatPatch
    {
        private const string DefaultKey = "BYRDONIS_NEST.pages.EAT.description";
        private const string NewKey = "BYRDONIS_NEST.pages.EAT.sorceressDescription";
    
        [HarmonyTranspiler]
        private static List<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return new InstructionPatcher(instructions)
                .Match(new InstructionMatcher()
                    .ldloc_1()
                    .ldloc_1()
                    .ldstr(DefaultKey)
                    .call(typeof(EventModel), nameof(EventModel.L10NLookup), [typeof(string)])
                ).Step(-1).Insert([
                    CodeInstruction.LoadLocal(1),
                    CodeInstruction.Call(typeof(SorceressNestEatPatch), nameof(ReplaceEventText))
                ]);
        }
        
        private static string ReplaceEventText(string orig, ByrdonisNest instance)
        {
            return instance.Owner?.Character is Character.TheSorceressMod ? NewKey : orig;
        }
    }

    
    // public static class SorceressByrdonisEatPatch
    // {
    //     [HarmonyPrefix]
    //     public static void Prefix(ByrdonisNest __instance)
    //     {
    //         if (__instance.Owner != null && __instance.Owner.Character is Character.TheSorceressMod)
    //         {
    //             EventFinished?.Invoke(__instance, new object?[1] { new LocString("events", "BYRDONIS_NEST.pages.EAT.sorceressDescription") });
    //         }
    //     }
    // }
    
    [HarmonyPatch(typeof(ByrdonisNest),"Take", MethodType.Async)]
    public class SorceressNestTakePatch
    {
        private const string DefaultKey = "BYRDONIS_NEST.pages.TAKE.description";
        private const string NewKey = "BYRDONIS_NEST.pages.TAKE.sorceressDescription";
    
        [HarmonyTranspiler]
        private static List<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return new InstructionPatcher(instructions)
                .Match(new InstructionMatcher()
                    .ldloc_1()
                    .ldloc_1()
                    .ldstr(DefaultKey)
                    .call(typeof(EventModel), nameof(EventModel.L10NLookup), [typeof(string)])
                ).Step(-1).Insert([
                    CodeInstruction.LoadLocal(1),
                    CodeInstruction.Call(typeof(SorceressNestTakePatch), nameof(ReplaceEventText))
                ]);
        }
        
        private static string ReplaceEventText(string orig, ByrdonisNest instance)
        {
            return instance.Owner?.Character is Character.TheSorceressMod ? NewKey : orig;
        }
    }
    
    
    // public static class SorceressByrdonisTakePatch
    // {
    //     [HarmonyPrefix]
    //     public static void Prefix(ByrdonisNest __instance)
    //     {
    //         if (__instance.Owner != null && __instance.Owner.Character is Character.TheSorceressMod)
    //         {
    //             EventFinished?.Invoke(__instance, new object?[1] { new LocString("events", "BYRDONIS_NEST.pages.TAKE.sorceressDescription") });
    //         }
    //     }
    // }
    
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