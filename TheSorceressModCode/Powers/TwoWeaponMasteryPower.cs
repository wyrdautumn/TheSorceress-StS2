using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace TheSorceressMod.TheSorceressModCode.Powers;

public class TwoWeaponMasteryPower : TheSorceressModPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override int ModifyCardPlayCount(CardModel card, Creature? target, int playCount)
    {
        if (card.Owner.Creature == this.Owner && card.Tags.Contains(SorceressKeywords.TwoWeapon))
        {
            return playCount + Amount;
        }
        return playCount;
    }

    public override int ModifyAttackHitCount(AttackCommand attack, int hitCount)
    {
        if (attack.ModelSource is not CardModel)
        {
            return hitCount;
        }

        CardModel card = (CardModel) attack.ModelSource;

        if (card.Owner != this.Owner.Player || !card.Tags.Contains(SorceressKeywords.TwoWeapon))
        {
            return hitCount;
        }

        this.Flash();
        return hitCount + Amount;
    }
}