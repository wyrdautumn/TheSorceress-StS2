using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Cards;
using TheSorceressMod.TheSorceressModCode.Powers;

namespace TheSorceressMod.TheSorceressModCode.Cards.CrossMod.HeroExpansion;

public class ProbingStrike() : TheSorceressModHeroExpansionCard(1,
    CardType.Attack, CardRarity.Common,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(8, ValueProp.Move), new DynamicVar("Stealthy", 2)];
    protected override HashSet<CardTag> CanonicalTags =>
    [
        CardTag.Strike
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [..HoverTipFactory.FromEnchantment<StealthyEnchantment>(DynamicVars["Stealthy"].IntValue),..AddHeroExpansion()];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play,vfx:"vfx/vfx_attack_slash").Execute(choiceContext);
        CardSelectorPrefs prefs = new CardSelectorPrefs(SelectionScreenPrompt, 1);
        CardModel? card = (await CardSelectCmd.FromHand(choiceContext, Owner, prefs, (Func<CardModel, bool>) (card => card.Type == CardType.Attack && card.Enchantment == null), this)).FirstOrDefault();
        if (card == null)
            return;
        CardCmd.Enchant<StealthyEnchantment>(card, DynamicVars["Stealthy"].BaseValue);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
        DynamicVars["Stealthy"].UpgradeValueBy(1);
    }
}