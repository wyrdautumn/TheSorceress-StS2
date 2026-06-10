using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Cards;

namespace TheSorceressMod.TheSorceressModCode.Cards.Rare;

public class PressTheAdvantage() : TheSorceressModCard(1,
    CardType.Attack, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(6, ValueProp.Move),
    new CalculationBaseVar(1),
    new CalculationExtraVar(1),
    new CalculatedVar("hits").WithMultiplier(Calc)];

    private static decimal Calc(CardModel card, Creature? target)
    {
        if (target == null)
            return 0;
        return target.Powers.Count(ShouldCountPower);
    }


    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    { 
        await CommonActions.CardAttack(this, play, (int) ((CalculatedVar) DynamicVars["hits"]).Calculate(play.Target),vfx: "vfx/vfx_attack_slash").Execute(choiceContext);
    }
    
    private static bool ShouldCountPower(PowerModel power)
    {
        return power.TypeForCurrentAmount == PowerType.Debuff && !(power is ITemporaryPower);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
    }
}