using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;
using MegaCrit.Sts2.Core.Timeline;
using TheSorceressMod.TheSorceressModCode.Potions;

namespace TheSorceressMod.TheSorceressModCode.Epochs;

public class Sorceress1Epoch : EpochModel
{
    public override string Id => "THESORCERESSMOD-SORCERESS1_EPOCH";
    public override EpochEra Era => EpochEra.Invitation0;
    public override int EraPosition => 4;
    public override string? StoryId => "Sorceress";

    public static List<PotionModel> Potions =>
    [
        ModelDb.Potion<InvisibilityPotion>(),
        ModelDb.Potion<CharismaPotion>(),
        ModelDb.Potion<ShadowPotion>()
    ];

    public override string UnlockText => CreatePotionUnlockText(Potions);

    public override void QueueUnlocks()
    {
        NTimelineScreen.Instance.QueuePotionUnlock(Potions);
        NTimelineScreen.Instance.QueueMiscUnlock(new LocString("epochs", this.Id + ".unlock").GetFormattedText() ?? "");

    }
}