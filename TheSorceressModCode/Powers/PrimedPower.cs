using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheSorceressMod.TheSorceressModCode.Powers;

public class PrimedPower : TheSorceressModPower
{
    public static readonly SpireField<Creature, int> PrimeRemoved = new(() => 0);
    
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override object InitInternalData() => new Data();
    
    public override Task BeforeAttack(AttackCommand command)
    {
        if (command.IsSingleTargeted && command.CardPlay != null && command.CardPlay.Target != Owner || !command.DamageProps.IsPoweredAttack())
            return Task.CompletedTask;
        foreach (Creature enemy in CombatState.GetOpponentsOf(Owner))
        {
            if (enemy.HasPower<PersistentPrimePower>())
            {
                return Task.CompletedTask;
            }
        }
        Data internalData = GetInternalData<Data>();
        if (internalData.commandToModify != null || command.ModelSource != null && !(command.ModelSource is CardModel) || !command.DamageProps.IsPoweredAttack())
            return Task.CompletedTask;
        internalData.commandToModify = command;
        internalData.amountWhenAttackStarted = Amount;
        return Task.CompletedTask;
    }

    public override decimal ModifyDamageAdditive(Creature? target, decimal amount, ValueProp props, Creature? dealer,
        CardModel? cardSource, CardPlay? cardPlay)
    {
        if (target != this.Owner || !props.IsPoweredAttack() || cardSource == null)
            return 0;
        Data internalData = GetInternalData<Data>();
        foreach (Creature enemy in CombatState.GetOpponentsOf(Owner))
        {
            if (enemy.HasPower<PersistentPrimePower>())
            {
                return Amount;
            }
        }
        return internalData.commandToModify != null && cardSource != null && cardSource != internalData.commandToModify.ModelSource || internalData.commandToModify != null && internalData.commandToModify.Attacker != dealer ? 0M : Amount;
    }

    public override async Task AfterAttack(PlayerChoiceContext choiceContext, AttackCommand command)
    {
        Data internalData = GetInternalData<Data>();
        if (command.ModelSource is not CardModel || !command.Results.SelectMany(r => r.Select(c => c.Receiver)).Contains(Owner) || !command.DamageProps.IsPoweredAttack())
        {
            if (internalData.commandToModify != null && internalData.commandToModify == command)
                internalData.commandToModify = null;
            return;
        }
        foreach (Creature enemy in CombatState.GetOpponentsOf(Owner))
        {
            if (enemy.HasPower<PersistentPrimePower>())
            {
                await TriggerDetonator(choiceContext);
                return;
            }
        }
        CardModel card = (CardModel) command.ModelSource;
        if (command != internalData.commandToModify)
            return;
        if (!card.Tags.Contains(SorceressKeywords.PrimeTrick))
        {
            NCreature? creatureNode = NCombatRoom.Instance?.GetCreatureNode(Owner);
            if (creatureNode != null)
            {
                NFireBurstVfx? child = NFireBurstVfx.Create(creatureNode.GetBottomOfHitbox(), 1f, new Color("8263c0"));
                if (child != null)
                {
                    SfxCmd.Play("event:/sfx/characters/attack_fire");
                    NCombatRoom? instance = NCombatRoom.Instance;
                    if (instance != null)
                        instance.CombatVfxContainer.AddChildSafely((Godot.Node)child);
                }
            }
            PrimeRemoved.Set(Owner, internalData.amountWhenAttackStarted + Amount);
            await PowerCmd.ModifyAmount(choiceContext, this, -internalData.amountWhenAttackStarted, null, null);
        }
        else
        {
            internalData.commandToModify = null;
        }

        await TriggerDetonator(choiceContext);
    }

    private async Task TriggerDetonator(PlayerChoiceContext choiceContext)
    {
        DetonatorPower? detonator = Owner.GetPower<DetonatorPower>();
        if (detonator != null)
        {
            detonator.TriggerFlash();
            await PowerCmd.Apply<PrimedPower>(choiceContext, Owner, detonator.Amount, null, null);
            await PowerCmd.Remove(detonator);
        }
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side != CombatSide.Player)
            return;
        bool shouldRemove = false;
        foreach (Creature enemy in CombatState.GetOpponentsOf(Owner))
        {
            if (enemy.HasPower<PersistentPrimePower>())
            {
                shouldRemove = true;
                int loops = enemy.GetPowerAmount<PersistentPrimePower>();
                for(int i = loops; i > 0; --i)
                {
                    NCreature? creatureNode = NCombatRoom.Instance?.GetCreatureNode(Owner);
                    if (creatureNode != null)
                    {
                        NFireBurstVfx? child = NFireBurstVfx.Create(creatureNode.GetBottomOfHitbox(), 1f, new Color("8263c0"));
                        if (child != null)
                        {
                            SfxCmd.Play("event:/sfx/characters/attack_fire");
                            NCombatRoom? instance = NCombatRoom.Instance;
                            if (instance != null)
                                instance.CombatVfxContainer.AddChildSafely((Godot.Node)child);
                        }
                    }
                    await CreatureCmd.Damage(choiceContext, Owner, Amount, ValueProp.Unpowered, null, null);
                }
            }
        }
        if (shouldRemove)
        {
            int val = PrimeRemoved.Get(Owner);
            PrimeRemoved.Set(Owner, val + Amount);
            await PowerCmd.Remove(this);
        }
    }

    public override Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
    {
        if (creature == Owner && wasRemovalPrevented)
        {
            int val = PrimeRemoved.Get(Owner);
            PrimeRemoved.Set(Owner, val + Amount);
        }
        return Task.CompletedTask;
    }
    
    private class Data
    {
        public AttackCommand? commandToModify;
        public int amountWhenAttackStarted;
    }
}


