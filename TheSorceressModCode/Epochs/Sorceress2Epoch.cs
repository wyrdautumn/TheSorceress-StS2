using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;
using MegaCrit.Sts2.Core.Timeline;
using TheSorceressMod.TheSorceressModCode.Potions;
using TheSorceressMod.TheSorceressModCode.Relics;

namespace TheSorceressMod.TheSorceressModCode.Epochs;

public class Sorceress2Epoch : EpochModel
{
    public override string Id => "THESORCERESSMOD-SORCERESS2_EPOCH";
    public override EpochEra Era => EpochEra.Invitation2;
    public override int EraPosition => 2;
    public override string? StoryId => "Sorceress";

    public static List<RelicModel> Relics =>
    [
        ModelDb.Relic<Voidheart>(),
        ModelDb.Relic<DancingSash>(),
        ModelDb.Relic<WickedDagger>()
    ];

    public override string UnlockText => CreateRelicUnlockText(Relics);

    public override void QueueUnlocks()
    {
        NTimelineScreen.Instance.QueueRelicUnlock(Relics);
    }
}