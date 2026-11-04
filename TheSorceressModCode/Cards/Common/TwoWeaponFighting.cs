using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Cards;
using TheSorceressMod.TheSorceressModCode.Cards.Tokens;

namespace TheSorceressMod.TheSorceressModCode.Cards.Common;

public class TwoWeaponFighting() : TheSorceressModCard(1,
    CardType.Attack, CardRarity.Common,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(3, ValueProp.Move)];
    protected override HashSet<CardTag> CanonicalTags
    {
        get => new HashSet<CardTag>() { SorceressKeywords.TwoWeapon };
    }
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [FromCombinationWithHitCount(this)];

    private static IHoverTip FromCombinationWithHitCount(CardModel card)
    {
        if (card.CombatState == null)
            return HoverTipFactory.FromCard<TwoWeaponCombination>();
        CardModel preview = (CardModel) ModelDb.Card<TwoWeaponCombination>().MutableClone();
        int plays = CombatManager.Instance.History.CardPlaysFinished.Count<CardPlayFinishedEntry>(e =>
            e.CardPlay.Card is TwoWeaponFighting && e.CardPlay.Card.Owner == card.Owner);
        plays += 1;
        preview.DynamicVars.CalculationBase.UpgradeValueBy(plays);
        if (card.IsUpgraded)
            preview.UpgradeInternal();
        preview.FinalizeUpgradeInternal();
        preview.DynamicVars.RecalculateForUpgradeOrEnchant();
        preview.UpdateDynamicVarPreview(CardPreviewMode.Normal, null, preview.DynamicVars);
        return new CardHoverTip(preview);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play, 2, vfx: "vfx/vfx_attack_slash").Execute(choiceContext);
        if (CombatState == null)
            return;
        CardModel combination = CombatState.CreateCard<TwoWeaponCombination>(Owner);
        await CardPileCmd.AddGeneratedCardToCombat(combination, PileType.Hand, Owner);
        if (IsUpgraded)
            CardCmd.Upgrade(combination);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1);
    }
}