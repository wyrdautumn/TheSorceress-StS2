using BaseLib.Abstracts;
using BaseLib.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Cards.CrossMod.HeroExpansion;
using TheSorceressMod.TheSorceressModCode.Extensions;
using TheSorceressMod.TheSorceressModCode.helpers;
using TheSorceressMod.TheSorceressModCode.Relics;

namespace TheSorceressMod.TheSorceressModCode.Powers;

public class CombatAdvantagePower : TheSorceressModPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    
    protected override object InitInternalData() => new Data();
    
    public override Task BeforeAttack(AttackCommand command)
    {
        if (command.Attacker != Owner || !command.DamageProps.IsPoweredAttack())
            return Task.CompletedTask;
        Data internalData = GetInternalData<Data>();
        if (internalData.commandToModify != null || command.ModelSource != null && !(command.ModelSource is CardModel) || !command.DamageProps.IsPoweredAttack())
            return Task.CompletedTask;
        internalData.commandToModify = command;
        return Task.CompletedTask;
    }
    
    public override Decimal ModifyDamageMultiplicative(
        Creature? target,
        Decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource,
        CardPlay? cardPlay)
    {
        if (dealer is null) return 1;
        if (dealer != this.Owner || !props.IsPoweredAttack() ||
            cardSource == null)
        {
            return 1;
        }
        Data internalData = GetInternalData<Data>();

        if (internalData.commandToModify != null && (internalData.commandToModify.ModelSource != cardSource || internalData.commandToModify.Attacker != dealer))
            return 1;
        
        WickedDagger? relic = Owner.Player?.GetRelic<WickedDagger>();
        if (relic != null)
        {
            return 1.75M;
        }
        return 1.5M;
    }

    public override async Task AfterAttack(PlayerChoiceContext choiceContext, AttackCommand command)
    {
        Data internalData = GetInternalData<Data>();
        if (command != internalData.commandToModify)
            return;

        if (command.ModelSource is CardModel cardSource && (cardSource.Tags.Contains(SorceressKeywords.Stealthy) || cardSource.Enchantment is StealthyEnchantment))
        {
            internalData.commandToModify = null;
            return;
        }
        await PowerCmd.Remove(this);
    }

    public override async Task AfterRemoved(Creature oldOwner)
    {
        SorcerousSpark? relic = Owner.Player?.GetRelic<SorcerousSpark>();
        if (relic != null && Owner.CombatState != null)
        {
            relic.Flash();
            await PowerCmd.Apply<CharismaPower>(new BlockingPlayerChoiceContext(), Owner,
                relic.DynamicVars["CharismaPower"].BaseValue, Owner, null);
        }
    }
    
    private class Data
    {
        public AttackCommand? commandToModify;
    }
}