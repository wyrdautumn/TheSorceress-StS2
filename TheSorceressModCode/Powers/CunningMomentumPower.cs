using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace TheSorceressMod.TheSorceressModCode.Powers;

public class CunningMomentumPower : TheSorceressModPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword(SorceressKeywords.Sleight),HoverTipFactory.ForEnergy(this)];
    
    protected override object InitInternalData() => (object) new CunningMomentumPower.Data();

    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != this.Owner.Player)
        {
            return Task.CompletedTask;
        }

        CunningMomentumPower.Data internalData = GetInternalData<CunningMomentumPower.Data>();
        internalData.cardPlayed = cardPlay.Card;
        internalData.amountWhenCardPlayed = Amount;
        return Task.CompletedTask;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CunningMomentumPower power = this;
        CunningMomentumPower.Data internalData = GetInternalData<CunningMomentumPower.Data>();
        if (cardPlay.Card.Keywords.Contains(SorceressKeywords.Sleight) && internalData.cardPlayed == cardPlay.Card)
        {
            await PlayerCmd.GainEnergy((Decimal)internalData.amountWhenCardPlayed, cardPlay.Card.Owner);
            await PowerCmd.ModifyAmount(choiceContext, power, -internalData.amountWhenCardPlayed, null, null);
        }
    }
    
    private class Data
    {
        public CardModel? cardPlayed;
        public int amountWhenCardPlayed;
    }
}