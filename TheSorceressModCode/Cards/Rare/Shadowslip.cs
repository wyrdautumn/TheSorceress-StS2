using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheSorceressMod.TheSorceressModCode.Cards;

namespace TheSorceressMod.TheSorceressModCode.Cards.Rare;

public class Shadowslip() : TheSorceressModCard(3,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword(SorceressKeywords.Subtle),HoverTipFactory.FromKeyword(SorceressKeywords.Shadowdance)];
    

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        foreach (CardModel card in PileType.Hand.GetPile(this.Owner).Cards.ToList<CardModel>())
        {
            if ((card.Type == CardType.Attack || card.Type == CardType.Skill) && !card.Keywords.Contains(SorceressKeywords.Subtle))
                CardCmd.ApplyKeyword(card, SorceressKeywords.Subtle);
            if ((card.Type == CardType.Attack || card.Type == CardType.Skill) && !card.Keywords.Contains(SorceressKeywords.Shadowdance))
                CardCmd.ApplyKeyword(card, SorceressKeywords.Shadowdance);
            await CardCmd.Exhaust(choiceContext, card);
        }
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}