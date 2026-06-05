using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;
using MegaCrit.Sts2.Core.Timeline;
using TheSorceressMod.TheSorceressModCode.Relics;

namespace TheSorceressMod.TheSorceressModCode.Epochs;

public class Sorceress6Epoch : EpochModel
{
    public override string Id => "THESORCERESSMOD-SORCERESS6_EPOCH";
    public override EpochEra Era => EpochEra.Invitation6;
    public override int EraPosition => 2;
    public override string? StoryId => "Sorceress";
    
    public static List<RelicModel> Relics =>
    [
        ModelDb.Relic<FetchingHat>(),
        ModelDb.Relic<DuelingSword>(),
        ModelDb.Relic<ThrowingKnives>()
    ];

    public override string UnlockText => CreateRelicUnlockText(Relics);

    public override void QueueUnlocks()
    {
        NTimelineScreen.Instance.QueueRelicUnlock(Relics);
    }
}