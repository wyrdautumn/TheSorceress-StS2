using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace TheSorceressMod.TheSorceressModCode.Powers;

public class LegendarySneakPower : TheSorceressModPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword(SorceressKeywords.Sleight),HoverTipFactory.FromPower<CombatAdvantagePower>()];

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature != Owner || !cardPlay.Card.Keywords.Contains(SorceressKeywords.Sleight))
        {
            return;
        }
        await PowerCmd.Apply<CombatAdvantagePower>(choiceContext, Owner, 1, Owner, null);
    }
}