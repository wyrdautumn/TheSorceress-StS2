using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Rooms;
using TheSorceressMod.TheSorceressModCode.Powers;
using TheSorceressMod.TheSorceressModCode.Relics;

namespace TheSorceressMod.TheSorceressModCode.Relics;

public class SorcerousSpark() : TheSorceressModRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Starter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<CombatAdvantagePower>(), HoverTipFactory.FromPower<CharismaPower>()];
    
    public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (combatState.RoundNumber != 1 || side != CombatSide.Player || !participants.Contains(Owner.Creature))
            return;
        Flash();
        await PowerCmd.Apply<CombatAdvantagePower>(choiceContext, Owner.Creature, 1, Owner.Creature, null);
        await PowerCmd.Apply<CharismaPower>(choiceContext, this.Owner.Creature, 4, this.Owner.Creature, null);
    }
}