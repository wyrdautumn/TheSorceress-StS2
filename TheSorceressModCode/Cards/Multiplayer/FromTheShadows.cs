using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheSorceressMod.TheSorceressModCode.Cards;

namespace TheSorceressMod.TheSorceressModCode.Cards.Multiplayer;

public class FromTheShadows() : TheSorceressModCard(0,
    CardType.Skill, CardRarity.Rare,
    TargetType.AnyAlly)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [SorceressKeywords.Sleight, CardKeyword.Exhaust];
    
    public override CardMultiplayerConstraint MultiplayerConstraint
    {
        get => CardMultiplayerConstraint.MultiplayerOnly;
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (play.Target == null || play.Target.Player == null)
            return;
        List<CardModel> allyPile = new List<CardModel>();
        foreach (CardModel card in PileType.Exhaust.GetPile(play.Target.Player).Cards.Where(c => c.Type is CardType.Attack or CardType.Skill))
        {
            CardModel clone = card.CreateCloneForPlayer(Owner);
            allyPile.Add(clone);
        }
        var prefs = new CardSelectorPrefs(new LocString("card_selection", "TO_TELL"), 1);
        if (IsUpgraded)
        {
            var card = (await CardSelectCmd.FromSimpleGrid(
                choiceContext,
                allyPile,
                Owner,
                prefs)).FirstOrDefault();
            if (card == null)
                return;
            CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Draw, Owner, CardPilePosition.Top));
        }
        else
        {
            var card = await CardSelectCmd.FromChooseACardScreen(choiceContext, allyPile.TakeRandom(3,Owner.RunState.Rng.CombatCardSelection).ToList(), Owner);
            if (card == null)
                return;
            CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Draw, Owner, CardPilePosition.Top));
        }
    }

    protected override void OnUpgrade()
    {

    }
}