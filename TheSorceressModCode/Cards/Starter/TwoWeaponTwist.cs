using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Cards;
using TheSorceressMod.TheSorceressModCode.Powers;

namespace TheSorceressMod.TheSorceressModCode.Cards.Starter;

[Pool(typeof(TokenCardPool))]
public class TwoWeaponTwist() : TheSorceressModCard(1,
    CardType.Attack, CardRarity.Token,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(4, ValueProp.Move), new PowerVar<TwoWeaponTwistPower>(2)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<StrengthPower>()];

    protected override HashSet<CardTag> CanonicalTags
    {
        get => new HashSet<CardTag>() { SorceressKeywords.TwoWeapon };
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play, 1, vfx:"vfx/vfx_attack_slash").Execute(choiceContext);
        if (play.Target == null)
        {
            return;
        }
        await CommonActions.Apply<TwoWeaponTwistPower>(choiceContext, play.Target, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
        DynamicVars["TwoWeaponTwistPower"].UpgradeValueBy(1);
    }
}