using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using TheSorceressMod.TheSorceressModCode.Cards;

namespace TheSorceressMod.TheSorceressModCode.Powers;

public class NimbleFingersPower : TheSorceressModPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword(SorceressKeywords.Sleight)];
    
    public override int DisplayAmount
    {
        get
        {
            if (Owner.Player == null || Owner.Player.PlayerCombatState == null)
            {
                return Amount;
            }
            return Math.Max(0, this.Amount - SleightHelper.NimbleFingersCount.Get(Owner.Player.PlayerCombatState));
        }
    }

    public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner == this.Owner.Player && cardPlay.Card.Owner.PlayerCombatState != null && cardPlay.Card.Keywords.Contains(SorceressKeywords.Sleight))
        {
            var val = SleightHelper.NimbleFingersCount.Get(cardPlay.Card.Owner.PlayerCombatState);
            if (val < Amount)
            {
                this.Flash();
            }
        }
        InvokeDisplayAmountChanged();
        return Task.CompletedTask;
    }

    public override Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
    {
        if (player.Creature == Owner)
            InvokeDisplayAmountChanged();
        return Task.CompletedTask;
    }
}