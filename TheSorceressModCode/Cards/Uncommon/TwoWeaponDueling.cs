using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Cards;
using TheSorceressMod.TheSorceressModCode.Cards.Tokens;

namespace TheSorceressMod.TheSorceressModCode.Cards.Uncommon;

public class TwoWeaponDueling() : TheSorceressModCard(1,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.RandomEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(3, ValueProp.Move), new DynamicVar("hits", 2)];

    protected override HashSet<CardTag> CanonicalTags
    {
        get => new HashSet<CardTag>() { SorceressKeywords.TwoWeapon };
    }
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [..HoverTipFactory.FromCardWithCardHoverTips<TwoWeaponDervish>(IsUpgraded)];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play, DynamicVars["hits"].IntValue, vfx: "vfx/vfx_attack_slash")
            .Execute(choiceContext);
        if (CombatState == null)
            return;
        CardModel dervish = CombatState.CreateCard<TwoWeaponDervish>(Owner);
        await CardPileCmd.AddGeneratedCardToCombat(dervish, PileType.Hand, Owner);
        if (IsUpgraded)
            CardCmd.Upgrade(dervish);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["hits"].UpgradeValueBy(1);
    }
}