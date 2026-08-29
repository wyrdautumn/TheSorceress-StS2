using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Rooms;
using TheSorceressMod.TheSorceressModCode.Powers;
using TheSorceressMod.TheSorceressModCode.Relics;

namespace TheSorceressMod.TheSorceressModCode.Relics;

public class PrimalFlame() : TheSorceressModRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Uncommon;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<PrimedPower>(6)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<PrimedPower>()];

    public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (!participants.Contains(Owner.Creature) || Owner.PlayerCombatState == null || Owner.PlayerCombatState.TurnNumber > 1 || Owner.Creature.CombatState == null)
            return;
        Flash();
        foreach (Creature hittableEnemy in Owner.Creature.CombatState.HittableEnemies)
        {
            NCombatRoom? instance = NCombatRoom.Instance;
            if (instance != null)
                instance.CombatVfxContainer.AddChildSafely(NSmokePuffVfx.Create(hittableEnemy, NSmokePuffVfx.SmokePuffColor.Purple));
        }
        await Cmd.CustomScaledWait(0.2f, 0.4f);
        await PowerCmd.Apply<PrimedPower>(choiceContext, Owner.Creature.CombatState.HittableEnemies, DynamicVars["PrimedPower"].IntValue, Owner.Creature, null);
    }
}