using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace TheSorceressMod.TheSorceressModCode.Powers;

public class SpellbladePower : TheSorceressModPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Single;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword(SorceressKeywords.Sorcery)];

    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        if (Owner.Player == null || Owner.Player.PlayerCombatState == null)
        {
            return Task.CompletedTask;
        }
        foreach (CardModel card in Owner.Player.PlayerCombatState.AllCards.Where<CardModel>((Func<CardModel, bool>) (c => c.Type == CardType.Attack)))
            CardCmd.ApplyKeyword(card, SorceressKeywords.Sorcery);
        return Task.CompletedTask;
    }

    public override Task AfterCardEnteredCombat(CardModel card)
    {
        if (card.Type != CardType.Attack || card.Owner != this.Owner.Player)
        {
            return Task.CompletedTask;
        }
        CardCmd.ApplyKeyword(card, SorceressKeywords.Sorcery);
        return Task.CompletedTask;
    }
}