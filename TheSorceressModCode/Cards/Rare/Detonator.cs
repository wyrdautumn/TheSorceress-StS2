using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Cards;
using TheSorceressMod.TheSorceressModCode.Powers;

namespace TheSorceressMod.TheSorceressModCode.Cards.Rare;

public class Detonator() : TheSorceressModCard(0,
    CardType.Attack, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    private Decimal _extraDamageFromPrime;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(0, ValueProp.Move), new DynamicVar("hits", 1)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<PrimedPower>()];
    
    private Decimal ExtraDamageFromPrime
    {
        get => _extraDamageFromPrime;
        set
        {
            AssertMutable();
            _extraDamageFromPrime = value;
        }
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (play.Target == null)
            return;
        int increase = play.Target.GetPowerAmount<PrimedPower>();
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this, play).WithHitCount(DynamicVars["hits"].IntValue).Targeting(play.Target).WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3")
            .Execute(choiceContext);
        DynamicVars.Damage.BaseValue += increase;
        ExtraDamageFromPrime += increase;
    }
    
    protected override void AfterDowngraded()
    {
        base.AfterDowngraded();
        DynamicVars.Damage.BaseValue += ExtraDamageFromPrime;
    }

    protected override void OnUpgrade()
    {
        DynamicVars["hits"].UpgradeValueBy(1);
    }
}