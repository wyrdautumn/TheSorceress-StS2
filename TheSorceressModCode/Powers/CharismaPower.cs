using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheSorceressMod.TheSorceressModCode.Powers;

public class CharismaPower : TheSorceressModPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override bool AllowNegative => true;

    public override Decimal ModifyDamageAdditive(
        Creature? target,
        Decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        if ((cardSource != null && cardSource.Keywords.Contains(SorceressKeywords.Sorcery) && dealer == Owner) || (dealer == Owner && props.HasFlag(ValueProp.Unblockable) && target != Owner))
        {
            return Amount;
        }
        return 0M;
    }
    
    public override Decimal ModifyBlockAdditive(
        Creature target,
        Decimal block,
        ValueProp props,
        CardModel? cardSource,
        CardPlay? cardPlay)
    {
        if (cardSource != null && cardSource.Keywords.Contains(SorceressKeywords.Sorcery) && props.IsPoweredCardOrMonsterMoveBlock() && cardSource.Owner.Creature == this.Owner && this.Owner == target)
        {
            return Amount;
        }
        return 0M;
    }
}
