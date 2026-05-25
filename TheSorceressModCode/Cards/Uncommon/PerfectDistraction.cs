using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Cards;

namespace TheSorceressMod.TheSorceressModCode.Cards.Uncommon;

public class PerfectDistraction() : TheSorceressModCard(0,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(4)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [SorceressKeywords.Sleight];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (Owner.PlayerCombatState == null)
            return;
        List<CardModel> list = (await CardSelectCmd.FromHandForDiscard(choiceContext, Owner, new CardSelectorPrefs(SelectionScreenPrompt, 0, 999999999), null, this)).ToList<CardModel>();
        await CardCmd.Discard(choiceContext, list);
        int draw = DynamicVars.Cards.IntValue;
        int hand = Owner.PlayerCombatState.Hand.Cards.Count;
        int amount = Math.Max(0, draw - hand);
        await CardPileCmd.Draw(choiceContext, amount, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }
}