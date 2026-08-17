using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Cards;

namespace TheSorceressMod.TheSorceressModCode.Cards.CrossMod.HeroExpansion;

[Pool(typeof(TokenCardPool))]
public class LeftHandWeapon() : TheSorceressModHeroExpansionCard(2,
    CardType.Attack, CardRarity.Token,
    TargetType.AnyEnemy)
{
    private Decimal _extraDamageFromRightHand;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(6, ValueProp.Move)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust,CardKeyword.Ethereal];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.Static(SorceressKeywords.HeroExpansion)];
    protected override HashSet<CardTag> CanonicalTags
    {
        get => new HashSet<CardTag>() { SorceressKeywords.TwoWeapon };
    }
    
    private Decimal ExtraDamageFromRightHand
    {
        get => this._extraDamageFromRightHand;
        set
        {
            this.AssertMutable();
            this._extraDamageFromRightHand = value;
        }
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (play.Target == null)
            return;
        await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard((CardModel) this, play).Targeting(play.Target).WithHitFx(tmpSfx: "heavy_attack.mp3").WithHitVfxNode((Func<Creature, Node2D>) (t => NBigSlashVfx.Create(t) ?? throw new InvalidOperationException())).WithHitVfxNode((Func<Creature, Node2D>) (t => NBigSlashImpactVfx.Create(t) ?? throw new InvalidOperationException())).Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(6);
    }
    
    protected override void AfterDowngraded()
    {
        base.AfterDowngraded();
        DamageVar damage = this.DynamicVars.Damage;
        damage.BaseValue = damage.BaseValue + this.ExtraDamageFromRightHand;
    }

    public void BuffFromDiscard(Decimal extraDamage)
    {
        DamageVar damage = this.DynamicVars.Damage;
        damage.BaseValue = damage.BaseValue + extraDamage;
        this.ExtraDamageFromRightHand += extraDamage;
    }
}