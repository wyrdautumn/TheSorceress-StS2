using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace TheSorceressMod.TheSorceressModCode.Powers;

public class FirestarterPower : TheSorceressModPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<PrimedPower>()];

    public override async Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (!participants.Contains(Owner))
            return;
        Flash();
        await Cmd.CustomScaledWait(0.2f, 0.4f);
        foreach (Creature hittableEnemy in CombatState.HittableEnemies)
        {
            NCreature? creatureNode = NCombatRoom.Instance?.GetCreatureNode(hittableEnemy);
            if (creatureNode != null)
                NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(NGaseousImpactVfx.Create(creatureNode.VfxSpawnPosition, new Godot.Color("6c43c7")));
        }
        await PowerCmd.Apply<PrimedPower>(new ThrowingPlayerChoiceContext(), CombatState.HittableEnemies, Amount, Owner, null);
    }
}