using BaseLib.Abstracts;
using TheSorceressMod.TheSorceressModCode.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Unlocks;
using TheSorceressMod.TheSorceressModCode.Epochs;

namespace TheSorceressMod.TheSorceressModCode.Character;

public class TheSorceressModRelicPool : CustomRelicPoolModel
{
    public override Color LabOutlineColor => TheSorceressMod.Color;

    public override string BigEnergyIconPath => "charui/kalkara_big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/kalkara_text_energy.png".ImagePath();
    
    public override IEnumerable<RelicModel> GetUnlockedRelics(UnlockState unlockState)
    {
        var list = AllRelics.ToList();
        if (TheSorceressModConfig.AllStuffUnlocked == false)
        {
            if (!unlockState.IsEpochRevealed<Sorceress2Epoch>())
            {
                list.RemoveAll(r => Sorceress2Epoch.Relics.Any(relic => relic.Id == r.Id));
            }

            if (!unlockState.IsEpochRevealed<Sorceress6Epoch>())
            {
                list.RemoveAll(r => Sorceress6Epoch.Relics.Any(relic => relic.Id == r.Id));
            }
        }

        return list;
    }
}