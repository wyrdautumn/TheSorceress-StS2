using BaseLib.Cards.Variables;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheSorceressMod.TheSorceressModCode.Powers;

public class SorcerersGambitPower : TheSorceressModPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("ChaMult", 0),
        new DynamicVar("DisplayBase", 0),
        new DynamicVar("DisplayExtra", 1),
        new CustomCalculatedVar("Display").WithMultiplier(Calc)];
    
    private static decimal Calc(PowerModel power, Creature? arg2)
    {
        return Math.Max(0, power.Amount + power.Owner.GetPowerAmount<CharismaPower>() * power.DynamicVars["ChaMult"].IntValue);
    }
    
    public override int DisplayAmount => Math.Max(0, Amount + Owner.GetPowerAmount<CharismaPower>() * DynamicVars["ChaMult"].IntValue);

    public override Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier,
        CardModel? cardSource)
    {
        if (power == this && amount > 0)
            DynamicVars["ChaMult"].BaseValue += 1;
        if (power.Owner == Owner)
            InvokeDisplayAmountChanged();
        return Task.CompletedTask;
    }

    public override async Task AfterDamageReceived(
        PlayerChoiceContext choiceContext,
        Creature target,
        DamageResult _,
        ValueProp props,
        Creature? dealer,
        CardModel? __)
    {
        if (target != this.Owner || dealer == null || !props.IsPoweredAttack() || _.UnblockedDamage > 0)
        {
            return;
        }
        await CreatureCmd.Damage(choiceContext, dealer, Math.Max(0, Amount + Owner.GetPowerAmount<CharismaPower>() * DynamicVars["ChaMult"].IntValue), ValueProp.Unpowered, Owner, null, null);
    }
}