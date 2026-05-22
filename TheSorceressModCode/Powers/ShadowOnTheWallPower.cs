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
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<CombatAdvantagePower>()];
    
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner.Player || cardPlay.Card.Type != CardType.Attack || this.Amount < 1)
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
    }
}