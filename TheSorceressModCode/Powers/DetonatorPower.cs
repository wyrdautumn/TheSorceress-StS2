using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace TheSorceressMod.TheSorceressModCode.Powers;

public class DetonatorPower : TheSorceressModPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword(SorceressKeywords.Subtle), HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];

    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (card.Owner.Creature == Owner && card.Keywords.Contains(SorceressKeywords.Subtle))
        {
            await CardCmd.Exhaust(choiceContext, card);
            await PowerCmd.Decrement(this);
        }
    }
}