using System.Collections;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheSorceressMod.TheSorceressModCode.Cards;

namespace TheSorceressMod.TheSorceressModCode.Cards.Rare;

public class TheGreatestLie() : TheSorceressModCard(0,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [SorceressKeywords.Sleight,CardKeyword.Exhaust];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var prefs = new CardSelectorPrefs(new LocString("card_selection", "TO_TELL"), DynamicVars.Cards.IntValue);
        var card = (await CardSelectCmd.FromSimpleGrid(
            choiceContext,
            PileType.Deck.GetPile(Owner).Cards,
            Owner,
            prefs)).FirstOrDefault();
        if (card == null || CombatState == null)
        {
            return;
        }
        var clone = CombatState?.CloneCard(card);
        if (clone != null)
        {
            CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(clone, PileType.Draw, Owner, CardPilePosition.Top));
        }
    }
    protected override void OnUpgrade()
    {
        this.AddKeyword(SorceressKeywords.Shadowdance);
    }
}