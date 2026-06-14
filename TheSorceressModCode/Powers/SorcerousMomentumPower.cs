using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace TheSorceressMod.TheSorceressModCode.Powers;

public class SorcerousMomentumPower : TheSorceressModPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword(SorceressKeywords.Sorcery),HoverTipFactory.ForEnergy(this)];
    
    protected override object InitInternalData() => (object) new SorcerousMomentumPower.Data();

    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != this.Owner.Player || !cardPlay.Card.Keywords.Contains(SorceressKeywords.Sorcery))
        {
            return Task.CompletedTask;
        }

        Data internalData = GetInternalData<Data>();
        internalData.cardPlayed = cardPlay.Card;
        internalData.amountWhenCardPlayed = Amount;
        return Task.CompletedTask;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        SorcerousMomentumPower power = this;
        Data internalData = GetInternalData<Data>();
        if (internalData.cardPlayed == cardPlay.Card)
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