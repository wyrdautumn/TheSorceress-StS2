using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Managers;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.Timeline;
using TheSorceressMod.TheSorceressModCode.Epochs;

namespace TheSorceressMod.TheSorceressModCode.Patches;

/// <summary>
/// Once again I'm copying very directly from mremingtonb. I did have to make my own patch for the initial epoch.
/// </summary>
[HarmonyPatch]
public static class SorceressEpochUnlockPatches
{
    private static readonly MethodInfo? TryObtainMidRun = AccessTools.Method(
        typeof(ProgressSaveManager), "TryObtainEpochMidRun", (Type[])null, (Type[])null);

    private static readonly MethodInfo? TryObtainPostRun = AccessTools.Method(
        typeof(ProgressSaveManager), "TryObtainEpochPostRun", (Type[])null, (Type[])null);

    [HarmonyPatch(typeof(ProgressSaveManager), "ObtainCharUnlockEpoch")]
    [HarmonyPostfix]
    public static void SorceressUnlockEpochPostfix(ProgressSaveManager __instance, Player localPlayer, int act)
    {
        if (!(localPlayer.Character is Character.TheSorceressMod))
            return;

        try
        {
            EpochModel? epoch = act switch
            {
                0 => EpochModel.Get(EpochModel.GetId<Sorceress5Epoch>()),
                1 => EpochModel.Get(EpochModel.GetId<Sorceress6Epoch>()),
                2 => EpochModel.Get(EpochModel.GetId<Sorceress1Epoch>()),
                _ => null
            };

            if (epoch != null)
            {
                TryObtainMidRun?.Invoke(__instance, new object[2] { epoch, localPlayer });
                MainFile.Logger.Info($"Sorceress epoch obtained for completing Act {act + 1}");
            }
        }
        catch (Exception ex)
        {
            MainFile.Logger.Info($"Sorceress act epoch check: {ex.Message}");
        }
    }

    [HarmonyPatch(typeof(ProgressSaveManager), "CheckFifteenElitesDefeatedEpoch")]
    [HarmonyPostfix]
    public static void SorceressEliteEpochPostfix(ProgressSaveManager __instance, Player localPlayer)
    {
        if (!(localPlayer.Character is Character.TheSorceressMod))
            return;

        try
        {
            EpochModel epoch = EpochModel.Get(EpochModel.GetId<Sorceress3Epoch>());
            HashSet<ModelId> eliteEncounterIds = GetEliteEncounterIds();
            ProgressState? progress = SaveManager.Instance?.Progress;
            if (progress == null) return;
            int eliteWins = 0;

            foreach (EncounterStats stats in progress.EncounterStats.Values)
            {
                if (!eliteEncounterIds.Contains(stats.Id))
                    continue;

                foreach (FightStats fightStat in stats.FightStats)
                {
                    if (fightStat.Character == ((AbstractModel)localPlayer.Character).Id)
                    {
                        eliteWins += fightStat.Wins;
                        break;
                    }
                }
            }

            if (eliteWins >= 15)
            {
                TryObtainMidRun?.Invoke(__instance, new object[2] { epoch, localPlayer });
            }
        }
        catch (Exception ex)
        {
            MainFile.Logger.Info($"Sorceress elite epoch check: {ex.Message}");
        }
    }

    [HarmonyPatch(typeof(ProgressSaveManager), "CheckFifteenBossesDefeatedEpoch")]
    [HarmonyPostfix]
    public static void SorceressBossEpochPostfix(ProgressSaveManager __instance, Player localPlayer)
    {
        if (!(localPlayer.Character is Character.TheSorceressMod))
            return;

        try
        {
            EpochModel epoch = EpochModel.Get(EpochModel.GetId<Sorceress2Epoch>());
            HashSet<ModelId> bossEncounters = ModelDb.Acts
                .SelectMany((ActModel a) => a.AllBossEncounters.Select((EncounterModel e) => ((AbstractModel)e).Id))
                .ToHashSet();

            ProgressState? progress = SaveManager.Instance?.Progress;
            if (progress == null) return;
            int bossWins = 0;

            foreach (EncounterStats stats in progress.EncounterStats.Values)
            {
                if (!bossEncounters.Contains(stats.Id))
                    continue;

                foreach (FightStats fightStat in stats.FightStats)
                {
                    if (fightStat.Character == ((AbstractModel)localPlayer.Character).Id)
                    {
                        bossWins += fightStat.Wins;
                        break;
                    }
                }
            }

            if (bossWins >= 15)
            {
                TryObtainMidRun?.Invoke(__instance, new object[2] { epoch, localPlayer });
            }
        }
        catch (Exception ex)
        {
            MainFile.Logger.Info($"Sorceress boss epoch check: {ex.Message}");
        }
    }

    [HarmonyPatch(typeof(ProgressSaveManager), "CheckAscensionOneCompleted")]
    [HarmonyPostfix]
    public static void SorceressAscensionEpochPostfix(ProgressSaveManager __instance,
        SerializablePlayer serializablePlayer, SerializableRun serializableRun)
    {
        if (serializableRun.Ascension != 1)
            return;

        try
        {
            CharacterModel charModel = ModelDb.GetById<CharacterModel>(serializablePlayer.CharacterId);
            if (charModel is Character.TheSorceressMod)
            {
                EpochModel epoch = EpochModel.Get(EpochModel.GetId<Sorceress7Epoch>());
                TryObtainPostRun?.Invoke(__instance, new object[3] { epoch, serializablePlayer, serializableRun });
            }
        }
        catch (Exception ex)
        {
            MainFile.Logger.Info($"Sorceress ascension epoch check: {ex.Message}");
        }
    }

    [HarmonyPatch(typeof(ProgressSaveManager), "PostRunUnlockCharacterEpochCheck")]
    [HarmonyPostfix]
    public static void UnlockSorceressEpochPostfix(ProgressSaveManager __instance, SerializablePlayer serializablePlayer, SerializableRun serializableRun)
    {
        if (serializablePlayer.CharacterId != ModelDb.GetId<Character.TheSorceressMod>())
        {
            return;
        }
        
        try
        {
            EpochModel epoch = EpochModel.Get(EpochModel.GetId<Sorceress4Epoch>());
            TryObtainPostRun?.Invoke(__instance, new object[3] { epoch, serializablePlayer, serializableRun });
        }
        catch (Exception ex)
        {
            MainFile.Logger.Info($"Sorceress first run epoch check: {ex.Message}");
        }
    }

    [HarmonyPatch(typeof(SaveManager), "GetCardUnlockEpochIds")]
    [HarmonyPostfix]
    public static void CardEpochIdsPostfix(ref string[] __result)
    {
        try
        {
            List<string> list = new List<string>(__result)
            {
                EpochModel.GetId<Sorceress3Epoch>(),
                EpochModel.GetId<Sorceress5Epoch>(),
                EpochModel.GetId<Sorceress7Epoch>(),
            };
            __result = list.ToArray();
        }
        catch (Exception ex)
        {
            MainFile.Logger.Info($"Card epoch IDs patch: {ex.Message}");
        }
    }

    [HarmonyPatch(typeof(SaveManager), "GetRelicUnlockEpochIds")]
    [HarmonyPostfix]
    public static void RelicEpochIdsPostfix(ref string[] __result)
    {
        try
        {
            List<string> list = new List<string>(__result)
            {
                EpochModel.GetId<Sorceress2Epoch>(),
                EpochModel.GetId<Sorceress6Epoch>()
            };
            __result = list.ToArray();
        }
        catch (Exception ex)
        {
            MainFile.Logger.Info($"Relic epoch IDs patch: {ex.Message}");
        }
    }

    private static HashSet<ModelId> GetEliteEncounterIds()
    {
        try
        {
            MethodInfo method = AccessTools.Method(typeof(ProgressSaveManager), "GetEliteEncounters", (Type[])null, (Type[])null);
            if (method != null)
            {
                return (HashSet<ModelId>)method.Invoke(null, null);
            }
        }
        catch
        {
        }

        return ModelDb.Acts
            .SelectMany((ActModel a) => a.AllEliteEncounters.Select((EncounterModel e) => ((AbstractModel)e).Id))
            .ToHashSet();
    }
}