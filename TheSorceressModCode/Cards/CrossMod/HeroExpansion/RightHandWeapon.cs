using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheSorceressMod.TheSorceressModCode.Cards;
using TheSorceressMod.TheSorceressModCode.Powers;

namespace TheSorceressMod.TheSorceressModCode.Cards.CrossMod.HeroExpansion;

public class RightHandWeapon() : TheSorceressModHeroExpansionCard(0,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("DamageIncrease", 4), new CardsVar(1)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [SorceressKeywords.Sleight];
    protected override HashSet<CardTag> CanonicalTags
    {
        get => new HashSet<CardTag>() { SorceressKeywords.TwoWeapon };
    }
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromCard<LeftHandWeapon>(),HoverTipFactory.Static(SorceressKeywords.HeroExpansion)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.Draw(this, choiceContext);
        List<CardModel> list = (await CardSelectCmd.FromHandForDiscard(choiceContext, Owner, new CardSelectorPrefs(SelectionScreenPrompt, 0, 999999999), null, this)).ToList<CardModel>();
        decimal cards = list.Count;
        await CardCmd.Discard(choiceContext, list);
        decimal damage = cards * DynamicVars["DamageIncrease"].BaseValue;
        if (CombatState == null)
            return;
        LeftHandWeapon leftHand = CombatState.CreateCard<LeftHandWeapon>(Owner);
        await CardPileCmd.AddGeneratedCardToCombat(leftHand, PileType.Hand, Owner);
        leftHand.BuffFromDiscard(damage);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["DamageIncrease"].UpgradeValueBy(2);
    }
}