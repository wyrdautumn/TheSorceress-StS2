using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Timeline;
using TheSorceressMod.TheSorceressModCode.Cards.Starter;

namespace TheSorceressMod.TheSorceressModCode.Epochs;

public class Sorceress4Epoch : EpochModel
{
    public override string Id => "THESORCERESSMOD-SORCERESS4_EPOCH";
    public override EpochEra Era => EpochEra.Invitation4;
    public override int EraPosition => 2;
    public override string? StoryId => "Sorceress";
    
    public override EpochModel[] GetTimelineExpansion()
    {
        return new EpochModel[6]
        {
            EpochModel.Get(EpochModel.GetId<Sorceress1Epoch>()),
            EpochModel.Get(EpochModel.GetId<Sorceress2Epoch>()),
            EpochModel.Get(EpochModel.GetId<Sorceress3Epoch>()),
            EpochModel.Get(EpochModel.GetId<Sorceress5Epoch>()),
            EpochModel.Get(EpochModel.GetId<Sorceress6Epoch>()),
            EpochModel.Get(EpochModel.GetId<Sorceress7Epoch>()),
        };
    }
    
    public override void QueueUnlocks()
    {
        EpochModel.QueueTimelineExpansion(this.GetTimelineExpansion());
    }
}