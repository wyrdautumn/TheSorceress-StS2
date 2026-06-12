using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
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
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override decimal ModifyDamageAdditive(Creature? target, decimal amount, ValueProp props, Creature? dealer,
        CardModel? cardSource)
    {
        if (target != this.Owner || !props.IsPoweredAttack())
            return 0;
        return Amount;
    }

    public override async Task AfterAttack(PlayerChoiceContext choiceContext, AttackCommand command)
    {
        if (command.ModelSource is not CardModel || !command.Results.SelectMany(r => r.Select(c => c.Receiver)).Contains(Owner) || !command.DamageProps.IsPoweredAttack())
        {
            return;
        }
        CardModel card = (CardModel) command.ModelSource;
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
            await PowerCmd.Remove(this);
        }
    }
}


