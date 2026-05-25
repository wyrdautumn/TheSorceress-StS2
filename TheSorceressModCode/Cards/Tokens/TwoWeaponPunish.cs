using BaseLib.Extensions;
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

namespace TheSorceressMod.TheSorceressModCode.Cards.Tokens;

[Pool(typeof(TokenCardPool))]
public class TwoWeaponPunish() : TheSorceressModCard(0,
    CardType.Attack, CardRarity.Token,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(2, ValueProp.Move), new PowerVar<WeakPower>(2)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Ethereal, CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<CombatAdvantagePower>(),HoverTipFactory.FromPower<WeakPower>()];
    protected override HashSet<CardTag> CanonicalTags
    {
        get => new HashSet<CardTag>() { SorceressKeywords.TwoWeapon };
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play, vfx: "vfx/vfx_attack_slash").Execute(choiceContext);
        if (play.Target == null)
            return;
        await CommonActions.Apply<WeakPower>(choiceContext, play.Target, this);
    }

    protected override bool IsPlayable => Owner.HasPower<CombatAdvantagePower>();

    protected override void OnUpgrade()
    {
        DynamicVars.Weak.UpgradeValueBy(1);
    }
}