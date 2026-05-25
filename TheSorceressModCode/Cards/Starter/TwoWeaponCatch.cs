using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Cards;
using TheSorceressMod.TheSorceressModCode.Cards.Tokens;
using TheSorceressMod.TheSorceressModCode.Powers;

namespace TheSorceressMod.TheSorceressModCode.Cards.Starter;

public class TwoWeaponCatch() : TheSorceressModCard(1,
    CardType.Attack, CardRarity.Basic,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(4, ValueProp.Move), new PowerVar<WeakPower>(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<WeakPower>(),..HoverTipFactory.FromCardWithCardHoverTips<TwoWeaponTwist>()];
    protected override HashSet<CardTag> CanonicalTags
    {
        get => new HashSet<CardTag>() { SorceressKeywords.TwoWeapon };
    }
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play, 1, vfx:"vfx/vfx_attack_slash").Execute(choiceContext);
        if (play.Target == null)
        {
            return;
        }
        await CommonActions.Apply<WeakPower>(choiceContext, play.Target, this);
        if (CombatState == null)
            return;
        CardModel twist = CombatState.CreateCard<TwoWeaponTwist>(Owner);
        await CardPileCmd.AddGeneratedCardToCombat(twist, PileType.Hand, Owner);
        if (IsUpgraded)
            CardCmd.Upgrade(twist);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
        DynamicVars.Weak.UpgradeValueBy(1);
    }
}