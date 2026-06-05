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
    protected override object InitInternalData() => (object) new ShadowOnTheWallPower.Data();
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<CombatAdvantagePower>()];
    
    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner.Player || cardPlay.Card.Type != CardType.Attack || this.Amount < 1)
        {
            return Task.CompletedTask;
        }
        this.GetInternalData<ShadowOnTheWallPower.Data>().cardPlayed.Add(cardPlay.Card);
        return Task.CompletedTask;
    }
    
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner.Player || cardPlay.Card.Type != CardType.Attack || this.Amount < 1 || 
            !GetInternalData<ShadowOnTheWallPower.Data>().cardPlayed.Contains(cardPlay.Card))
        {
            return;
        }
        Creature? target;
        if (cardPlay.Target != null && cardPlay.Target.IsAlive)
        {
            target = cardPlay.Target;
        }
        else
        {
            target = null;
        }
        await PowerCmd.Decrement(this);
        await PowerCmd.Apply<CombatAdvantagePower>(choiceContext, Owner, 1, Owner, null);
        await CardCmd.AutoPlay(choiceContext, cardPlay.Card.CreateDupe(), target);
        GetInternalData<ShadowOnTheWallPower.Data>().cardPlayed.Remove(cardPlay.Card);
    }
    
    private class Data
    {
        public readonly List<CardModel> cardPlayed = new List<CardModel>();
    }
}