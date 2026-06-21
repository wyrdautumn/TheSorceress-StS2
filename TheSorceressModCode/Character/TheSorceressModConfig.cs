using BaseLib.Config;
using BaseLib.Config.UI;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Timeline;
using TheSorceressMod.TheSorceressModCode.Epochs;

namespace TheSorceressMod.TheSorceressModCode.Character;

public class TheSorceressModConfig : SimpleModConfig
{
    public static bool RandomBool { get; set; } = false;
    
    [ConfigButton("AllStuffUnlocked")]
    public static void UnlockAllStuff()
    {
        List<string> epochs =
        [
            EpochModel.GetId<Sorceress1Epoch>(),
            EpochModel.GetId<Sorceress2Epoch>(),
            EpochModel.GetId<Sorceress3Epoch>(),
            EpochModel.GetId<Sorceress4Epoch>(),
            EpochModel.GetId<Sorceress5Epoch>(),
            EpochModel.GetId<Sorceress6Epoch>(),
            EpochModel.GetId<Sorceress7Epoch>()
        ];
        foreach (string epochId in epochs.Except(SaveManager.Instance.Progress.Epochs.Where(e => e.State == EpochState.Revealed).Select( e => e.Id)))
            SaveManager.Instance.ObtainEpochOverride(epochId, EpochState.Revealed);
    }
}