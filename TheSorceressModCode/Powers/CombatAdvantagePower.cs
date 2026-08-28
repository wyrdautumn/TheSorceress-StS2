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
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
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
        CardModel? cardSource,
        CardPlay? cardPlay)
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

    public override async Task AfterAttack(PlayerChoiceContext choiceContext, AttackCommand command)
    {
        if (command.Attacker == this.Owner && command.ModelSource is CardModel && command.DamageProps.IsPoweredAttack())
        {
            if (command.ModelSource is CardModel cardSource && cardSource.Tags.Contains(SorceressKeywords.Stealthy))
            {
                return;
            }
            await PowerCmd.Remove(this);
        }
    }

    public override async Task AfterRemoved(Creature oldOwner)
    {
        ThrowingKnives? relic = Owner.Player?.GetRelic<ThrowingKnives>();
        if (relic != null)
        {
            relic.Flash();
            if (Owner.CombatState == null || Owner.Player == null)
                return;
            Creature? target =
                Owner.Player.RunState.Rng.CombatTargets.NextItem<Creature>(Owner.CombatState.HittableEnemies);
            if (target == null)
                return;
            NDebugAudioManager.Instance?.Play("dagger_throw.mp3");
            Node? child = NShivThrowVfx.Create(Owner, target, new Color("b18aff"));
            NCombatRoom? instance = NCombatRoom.Instance;
            if (instance != null && child != null)
                instance.CombatVfxContainer.AddChildSafely(child);
            await CreatureCmd.Damage(new BlockingPlayerChoiceContext(), target, relic.DynamicVars.Damage, Owner);
        }
    }
}