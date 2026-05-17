using MegaCrit.Sts2.Core.Entities.Cards;
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

    public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner == this.Owner.Player && cardPlay.Card.Owner.PlayerCombatState != null)
        {
            var val = SleightHelper.NimbleFingersCount.Get(cardPlay.Card.Owner.PlayerCombatState);
            if (val < Amount)
            {
                this.Flash();
            }
        }
        return Task.CompletedTask;
    }
}