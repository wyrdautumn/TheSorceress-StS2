using BaseLib.Abstracts;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Entities.Powers;
using TheSorceressMod.TheSorceressModCode.Cards.Uncommon;
using TheSorceressMod.TheSorceressModCode.Extensions;

namespace TheSorceressMod.TheSorceressModCode.Powers;

public class BonMotPower : CustomTemporaryPowerModelWrapper<BonMot, CharismaPower>
{
    public override PowerType Type => PowerType.Buff;
    public override string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
    public override string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
}