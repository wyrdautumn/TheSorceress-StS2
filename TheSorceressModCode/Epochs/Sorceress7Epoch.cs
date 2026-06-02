using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;
using MegaCrit.Sts2.Core.Timeline;
using TheSorceressMod.TheSorceressModCode.Cards.Common;
using TheSorceressMod.TheSorceressModCode.Cards.Rare;
using TheSorceressMod.TheSorceressModCode.Cards.Uncommon;

namespace TheSorceressMod.TheSorceressModCode.Epochs;

public class Sorceress7Epoch : EpochModel
{
    public override string Id => "THESORCERESSMOD-SORCERESS7_EPOCH";
    public override EpochEra Era => EpochEra.Invitation7;
    public override int EraPosition => 0;
    public override string? StoryId => "Sorceress";
    
    public static List<CardModel> Cards =>
    [
        ModelDb.Card<HellfireStrike>(),
        ModelDb.Card<HellfireSoul>(),
        ModelDb.Card<SoulRebel>()
    ];
    
    public override string UnlockText => CreateCardUnlockText(Cards);

    public override void QueueUnlocks()
    {
        NTimelineScreen.Instance.QueueCardUnlock(Cards);
    }
}