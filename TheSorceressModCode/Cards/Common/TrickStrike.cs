using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Cards;

namespace TheSorceressMod.TheSorceressModCode.Cards.Common;

public class TrickStrike() : TheSorceressModCard(1,
    CardType.Attack, CardRarity.Common,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(10,ValueProp.Move),new CardsVar(1)];
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Strike];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target,vfx:"vfx/vfx_attack_slash").Execute(choiceContext);
        var prefs = new CardSelectorPrefs(new LocString("card_selection", "TO_TELL"), DynamicVars.Cards.IntValue);
        var cards = await CardSelectCmd.FromSimpleGrid(choiceContext, PileType.Discard.GetPile(this.Owner).Cards,
            this.Owner, prefs);
        if (!IsUpgraded)
        {
            await CardPileCmd.Add(cards, PileType.Draw.GetPile(this.Owner), CardPilePosition.Random);
        }
        else
        {
            await CardPileCmd.Add(cards, PileType.Draw.GetPile(this.Owner), CardPilePosition.Top);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
    }
}