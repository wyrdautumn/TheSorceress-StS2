using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace TheSorceressMod.TheSorceressModCode.Powers;

public class StorytellersFormPower : TheSorceressModPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword(SorceressKeywords.Sorcery),HoverTipFactory.FromPower<CharismaPower>()];

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature != this.Owner || !cardPlay.Card.Keywords.Contains(SorceressKeywords.Sorcery))
            return;
        this.Flash();
        await PowerCmd.Apply<CharismaPower>(choiceContext, Owner, Amount, Owner, null);
    }
}