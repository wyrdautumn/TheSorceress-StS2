using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace TheSorceressMod.TheSorceressModCode.Powers;

public class ShadowOnTheWallPower : TheSorceressModPower
{
   
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override object InitInternalData() => new Data();
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<CombatAdvantagePower>()];

    public override int ModifyCardPlayCount(CardModel card, Creature? target, int playCount)
    {
        if (card.Owner.Creature == this.Owner && card.Type == CardType.Attack)
        {
            GetInternalData<Data>().CardPlayed.Add(card);
            return playCount + Amount;
        }
        return playCount;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (GetInternalData<Data>().CardPlayed.Contains(cardPlay.Card))
        {
            await PowerCmd.Apply<CombatAdvantagePower>(choiceContext, Owner, 1, Owner, null);
            await PowerCmd.Decrement(this);
            if (cardPlay.IsLastInSeries && Amount > 0)
                await PowerCmd.Remove(this);
        }
    }

    private class Data()
    {
        public List<CardModel> CardPlayed = new List<CardModel>();
    }
}