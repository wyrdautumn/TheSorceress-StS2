using MegaCrit.Sts2.Core.Timeline;

namespace TheSorceressMod.TheSorceressModCode.Epochs;

public class SorceressStory : StoryModel
{
    protected override string Id => "SORCERESS";

    public override EpochModel[] Epochs =>
    [
        EpochModel.Get(EpochModel.GetId<Sorceress1Epoch>()),
        EpochModel.Get(EpochModel.GetId<Sorceress2Epoch>()),
        EpochModel.Get(EpochModel.GetId<Sorceress3Epoch>()),
        EpochModel.Get(EpochModel.GetId<Sorceress4Epoch>()),
        EpochModel.Get(EpochModel.GetId<Sorceress5Epoch>()),
        EpochModel.Get(EpochModel.GetId<Sorceress6Epoch>()),
        EpochModel.Get(EpochModel.GetId<Sorceress7Epoch>()),
    ];
}