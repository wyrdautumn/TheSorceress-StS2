using BaseLib.Abstracts;
using BaseLib.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Extensions;
using TheSorceressMod.TheSorceressModCode.helpers;
using TheSorceressMod.TheSorceressModCode.Relics;

namespace TheSorceressMod.TheSorceressModCode.Powers;

public class CombatAdvantagePower : TheSorceressModPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    
    public override Decimal ModifyDamageMultiplicative(
        Creature? target,
        Decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        if (dealer is null) return 1;
        if (dealer != this.Owner || !props.IsPoweredAttack() ||
            cardSource == null)
        {
            return 1;
        }
        WickedDagger? relic = Owner.Player?.GetRelic<WickedDagger>();
        if (relic != null)
        {
            return 1.75M;
        }
        return 1.5M;
    }

    public override Task AfterAttack(PlayerChoiceContext choiceContext, AttackCommand command)
    {
        if (command.Attacker == this.Owner && command.ModelSource is CardModel && command.DamageProps.IsPoweredAttack())
        {
            CardModel? cardSource = command.ModelSource as CardModel;
            if (cardSource != null && cardSource.Tags.Contains(SorceressKeywords.Stealthy))
            {
                return Task.CompletedTask;
            }
            PowerCmd.Remove(this);
        }
        return Task.CompletedTask;
    }

    public override async Task AfterRemoved(Creature oldOwner)
    {
        SilverySwordbreaker? relic = Owner.Player?.GetRelic<SilverySwordbreaker>();
        if (relic != null)
        {
            relic.Flash();
            await CreatureCmd.GainBlock(Owner, 3, ValueProp.Unpowered, null, true);
        }
    }
}