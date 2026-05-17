using BaseLib.Abstracts;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models.Powers;
using TheSorceressMod.TheSorceressModCode.Cards.Uncommon;
using TheSorceressMod.TheSorceressModCode.Extensions;

namespace TheSorceressMod.TheSorceressModCode.Powers;

public class ShadowyTendrilsPower : CustomTemporaryPowerModelWrapper<ShadowyTendrils, StrengthPower>
{
    protected override bool InvertInternalPowerAmount => true;
    public override PowerType Type => PowerType.Debuff;
    public override string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
    public override string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
    
}