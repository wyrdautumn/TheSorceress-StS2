using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheSorceressMod.TheSorceressModCode.Cards;

namespace TheSorceressMod.TheSorceressModCode.Cards.CrossMod.HeroExpansion;

public class TopDeck() : TheSorceressModHeroExpansionCard(0,
    CardType.Skill, CardRarity.Common,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [SorceressKeywords.Sleight];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [..AddHeroExpansion()];


    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
        await CommonActions.Draw(this, choiceContext);
        CardSelectorPrefs prefs = new CardSelectorPrefs(new LocString("card_selection", "TO_TELL"), 1);
        CardModel? card2 = (await CardSelectCmd.FromHand(choiceContext, this.Owner, prefs, null, this)).FirstOrDefault<CardModel>();
        if (card2 == null)
            return;
        await CardPileCmd.Add(card2, PileType.Draw, CardPilePosition.Top);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }
}