using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace TheSorceressMod.TheSorceressModCode.Powers;

public class BurnOutPower : TheSorceressModPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword(CardKeyword.Exhaust), HoverTipFactory.ForEnergy(this)];
    
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner.Player)
            return;
        foreach (CardModel card in await CardSelectCmd.FromHand(choiceContext, player, new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, Amount), (Func<CardModel, bool>) null!, this))
            await CardCmd.Exhaust(choiceContext, card);
    }
    
    public override Decimal ModifyMaxEnergy(Player player, Decimal amount)
    {
        return player != Owner.Player ? amount : amount + Amount;
    }
}