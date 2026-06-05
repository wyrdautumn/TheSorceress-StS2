using BaseLib.Abstracts;
using TheSorceressMod.TheSorceressModCode.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Unlocks;
using TheSorceressMod.TheSorceressModCode.Epochs;

namespace TheSorceressMod.TheSorceressModCode.Character;

public class TheSorceressModPotionPool : CustomPotionPoolModel
{
    public override Color LabOutlineColor => TheSorceressMod.Color;


    public override string BigEnergyIconPath => "charui/kalkara_big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/kalkara_text_energy.png".ImagePath();
    
    protected override IEnumerable<PotionModel> GenerateAllPotions()
    {
        return (IEnumerable<PotionModel>) Sorceress1Epoch.Potions;
    }

    public override IEnumerable<PotionModel> GetUnlockedPotions(UnlockState unlockState)
    {
        if (!unlockState.IsEpochRevealed<Sorceress1Epoch>() && !TheSorceressModConfig.AllStuffUnlocked)
        {
            return Array.Empty<PotionModel>();
        }

        return GenerateAllPotions();
    }
}