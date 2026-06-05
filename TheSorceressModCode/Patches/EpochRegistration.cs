using System.Reflection;
using MegaCrit.Sts2.Core.Timeline;
using TheSorceressMod.TheSorceressModCode.Epochs;

namespace TheSorceressMod.TheSorceressModCode.Patches;

//I'm heavily lifting all this epoch stuff from mremingtonb's work on the Hermit port cuz I would not have figured it out myself.

public static class EpochRegistration
{
    internal static readonly Type[] SorceressEpochTypes =
    [
        typeof(Sorceress1Epoch),
        typeof(Sorceress2Epoch),
        typeof(Sorceress3Epoch),
        typeof(Sorceress4Epoch),
        typeof(Sorceress5Epoch),
        typeof(Sorceress6Epoch),
        typeof(Sorceress7Epoch)
    ];

    public static void RegisterEpochs()
    {
        RegisterEpochModels();
        RegisterStoryModel();
    }

    private static void RegisterEpochModels()
    {
        try
        {
            var epochType = typeof(EpochModel);
            var epochDictField = epochType.GetField("_epochTypeDictionary", BindingFlags.Static | BindingFlags.NonPublic);
            var typeToIdField = epochType.GetField("_typeToIdDictionary", BindingFlags.Static | BindingFlags.NonPublic);
            var allIdsField = epochType.GetField("_allEpochIds", BindingFlags.Static | BindingFlags.NonPublic);

            if (epochDictField == null || typeToIdField == null)
            {
                MainFile.Logger.Error("Could not find EpochModel static dictionaries via reflection!");
                return;
            }

            var epochDict = (Dictionary<string, Type>)epochDictField.GetValue(null)!;
            var typeToId = (Dictionary<Type, string>)typeToIdField.GetValue(null)!;

            foreach (var type in SorceressEpochTypes)
            {
                var epoch = (EpochModel)Activator.CreateInstance(type)!;
                string id = epoch.Id;

                if (epochDict.ContainsKey(id))
                {
                    MainFile.Logger.Info($"Epoch {id} already registered, skipping.");
                    continue;
                }

                epochDict[id] = type;
                typeToId[type] = id;
                MainFile.Logger.Info($"Registered epoch: {id}");
            }

            allIdsField?.SetValue(null, null);
            MainFile.Logger.Info($"Registered {SorceressEpochTypes.Length} Sorceress epochs successfully.");
        }
        catch (Exception ex)
        {
            MainFile.Logger.Error($"Failed to register Sorceress epochs: {ex}");
        }
    }

    private static void RegisterStoryModel()
    {
        try
        {
            var storyType = typeof(StoryModel);
            var storyDictField = storyType.GetField("_storyTypeDictionary", BindingFlags.Static | BindingFlags.NonPublic);

            if (storyDictField == null)
            {
                MainFile.Logger.Error("Could not find StoryModel._storyTypeDictionary via reflection!");
                return;
            }

            var storyDict = (Dictionary<string, Type>)storyDictField.GetValue(null)!;
            var story = new SorceressStory();
            var idProp = storyType.GetProperty("Id", BindingFlags.Instance | BindingFlags.NonPublic)!;
            string storyId = (string)idProp.GetValue(story)!;

            if (storyDict.ContainsKey(storyId))
            {
                MainFile.Logger.Info($"Story {storyId} already registered, skipping.");
                return;
            }

            storyDict[storyId] = typeof(SorceressStory);
            MainFile.Logger.Info($"Registered Sorceress story: {storyId}");
        }
        catch (Exception ex)
        {
            MainFile.Logger.Error($"Failed to register Sorceress story: {ex}");
        }
    }
}