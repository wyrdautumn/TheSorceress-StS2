using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheSorceressMod.TheSorceressModCode.Powers;

public class DefensiveRollPower : TheSorceressModPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];
    
    public override Decimal ModifyHpLostAfterOstyLate(
        Creature target,
        Decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        return target != this.Owner ? amount : 0M;
    }

    public override async Task AfterModifyingHpLostAfterOsty()
    {
        if (this.Owner.Player != null)
        {
            List<CardModel> piles = new List<CardModel>();
            piles.AddRange(PileType.Discard.GetPile(this.Owner.Player).Cards);
            piles.AddRange(PileType.Draw.GetPile(this.Owner.Player).Cards);
            foreach (CardModel card in piles.StableShuffle<CardModel>(this.Owner.Player.RunState.Rng.Shuffle).Take<CardModel>(5))
            {
                await CardCmd.Exhaust(new BlockingPlayerChoiceContext(), card);
            }
        }
        await PowerCmd.Decrement((PowerModel) this);
    }
}