using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;
using MegaCrit.Sts2.Core.Timeline;
using TheSorceressMod.TheSorceressModCode.Cards.Rare;
using TheSorceressMod.TheSorceressModCode.Cards.Uncommon;

namespace TheSorceressMod.TheSorceressModCode.Epochs;

public class Sorceress5Epoch : EpochModel
{
    public override string Id => "THESORCERESSMOD-SORCERESS5_EPOCH";
    public override EpochEra Era => EpochEra.Invitation5;
    public override int EraPosition => 4;
    public override string? StoryId => "Sorceress";
    
    public static List<CardModel> Cards =>
    [
        ModelDb.Card<FeintingFlurry>(),
        ModelDb.Card<NimbleFingers>(),
        ModelDb.Card<TrickstersAce>()
    ];
    
    public override string UnlockText => CreateCardUnlockText(Cards);

    public override void QueueUnlocks()
    {
        NTimelineScreen.Instance.QueueCardUnlock(Cards);
    }
}