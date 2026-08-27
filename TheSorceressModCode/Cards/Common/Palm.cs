using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheSorceressMod.TheSorceressModCode.Cards;

namespace TheSorceressMod.TheSorceressModCode.Cards.Common;

public class Palm() : TheSorceressModCard(0,
    CardType.Skill, CardRarity.Common,
    TargetType.Self)
{
    private List<CardModel> _rekindle = new List<CardModel>();
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [SorceressKeywords.Sleight];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.Static(SorceressKeywords.Rekindle)];
    
    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
    {
        if (player != Owner)
            return;
        foreach (CardModel card in _rekindle)
        {
            await CardPileCmd.Add(card, PileType.Hand.GetPile(Owner));
            card.SetToFreeThisTurn();
        }
        _rekindle.Clear();
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        CardModel? card = (await CardSelectCmd.FromHand(choiceContext, play.Card.Owner, new CardSelectorPrefs(new LocString("card_selection", "TO_PALM"), 1), null, play.Card)).FirstOrDefault();
        if (card == null)
            return;
        _rekindle.Add(card);
        await CardCmd.Exhaust(choiceContext, card);
    }

    protected override void AfterCloned()
    {
        _rekindle = new List<CardModel>();
    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(SorceressKeywords.Sleight);
    }
}