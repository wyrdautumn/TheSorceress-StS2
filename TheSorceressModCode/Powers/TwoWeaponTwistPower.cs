using BaseLib.Abstracts;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models.Powers;
using TheSorceressMod.TheSorceressModCode.Cards.Starter;
using TheSorceressMod.TheSorceressModCode.Extensions;

namespace TheSorceressMod.TheSorceressModCode.Powers;

public class TwoWeaponTwistPower : CustomTemporaryPowerModelWrapper<TwoWeaponTwist, StrengthPower>
{
    protected override bool InvertInternalPowerAmount => true;
    public override string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
    public override string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
}