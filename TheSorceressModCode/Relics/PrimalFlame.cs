using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Rooms;
using TheSorceressMod.TheSorceressModCode.Powers;
using TheSorceressMod.TheSorceressModCode.Relics;

namespace TheSorceressMod.TheSorceressModCode.Relics;

public class PrimalFlame() : TheSorceressModRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Uncommon;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<PrimedPower>()];

    public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (!participants.Contains(Owner.Creature) || Owner.PlayerCombatState == null || Owner.PlayerCombatState.TurnNumber > 1)
            return;
        this.Flash();
        await PowerCmd.Apply<PrimedPower>(choiceContext, combatState.HittableEnemies, 6, this.Owner.Creature, null);

    }
}