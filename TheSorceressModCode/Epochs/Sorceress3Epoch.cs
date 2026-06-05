using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;
using MegaCrit.Sts2.Core.Timeline;
using TheSorceressMod.TheSorceressModCode.Cards.Common;
using TheSorceressMod.TheSorceressModCode.Cards.Rare;
using TheSorceressMod.TheSorceressModCode.Cards.Uncommon;

namespace TheSorceressMod.TheSorceressModCode.Epochs;

public class Sorceress3Epoch : EpochModel
{
    public override string Id => "THESORCERESSMOD-SORCERESS3_EPOCH";
    public override EpochEra Era => EpochEra.Invitation3;
    public override int EraPosition => 2;
    public override string? StoryId => "Sorceress";

    public static List<CardModel> Cards =>
    [
        ModelDb.Card<ExplosivePyre>(),
        ModelDb.Card<ChaosSanctuary>(),
        ModelDb.Card<PrimeTheFire>()
    ];
    
    public override string UnlockText => CreateCardUnlockText(Cards);

    public override void QueueUnlocks()
    {
        NTimelineScreen.Instance.QueueCardUnlock(Cards);
    }
}