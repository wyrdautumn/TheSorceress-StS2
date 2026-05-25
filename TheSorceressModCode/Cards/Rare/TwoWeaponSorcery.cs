using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Cards;
using TheSorceressMod.TheSorceressModCode.Cards.Tokens;
using TheSorceressMod.TheSorceressModCode.Powers;

namespace TheSorceressMod.TheSorceressModCode.Cards.Rare;

public class TwoWeaponSorcery() : TheSorceressModCard(2,
    CardType.Attack, CardRarity.Rare,
    TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(6, ValueProp.Move)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [SorceressKeywords.Sorcery];
    protected override HashSet<CardTag> CanonicalTags
    {
        get => new HashSet<CardTag>() { SorceressKeywords.TwoWeapon };
    }
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [..HoverTipFactory.FromCardWithCardHoverTips<TwoWeaponBurst>(false)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this,play,2, vfx: "vfx/vfx_attack_slash").Execute(choiceContext);
        await PowerCmd.Apply<TwoWeaponSorceryPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
    }
}