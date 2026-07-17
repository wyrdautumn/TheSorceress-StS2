using BaseLib.Abstracts;
using BaseLib.Hooks;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Cards.Rare;

namespace TheSorceressMod.TheSorceressModCode.Powers;

public class PrimeTheFirePower : TheSorceressModPower
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override IEnumerable<HealthBarForecastSegment> GetHealthBarForecastSegments(HealthBarForecastContext context)
    {
        return [new HealthBarForecastSegment(Amount, Color.FromHtml("#b18aff"), HealthBarForecastDirection.FromRight)];
    }
    
    public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (!participants.Contains(Owner) || side != Owner.Side)
            return;
        await PurpleBurningEffect();
        await CreatureCmd.Damage(choiceContext, Owner, Amount, ValueProp.Unpowered | ValueProp.Unblockable, null, null, null);
        await PowerCmd.Remove(this);
    }

    private Task PurpleBurningEffect()
    {
        NCreature? creatureNode = NCombatRoom.Instance?.GetCreatureNode(Owner);
        if (creatureNode != null)
        {
            NFireBurningVfx? child =
                NFireBurningVfx.Create(creatureNode.GetBottomOfHitbox(), 1f, true, new Color("b18aff"));
            if (child == null)
                return Task.CompletedTask;
            SfxCmd.Play("event:/sfx/characters/attack_fire");
            NCombatRoom? instance = NCombatRoom.Instance;
            if (instance != null)
                instance.CombatVfxContainer.AddChildSafely((Godot.Node)child);
        }
        return Task.CompletedTask;
    }
}