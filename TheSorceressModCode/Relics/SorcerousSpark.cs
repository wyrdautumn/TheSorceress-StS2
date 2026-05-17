using MegaCrit.Sts2.Core.Commands;
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
        RelicRarity.Ancient;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<CombatAdvantagePower>(), HoverTipFactory.FromPower<CharismaPower>()];
    
    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        if (!(room is CombatRoom))
            return;
        this.Flash();
        await PowerCmd.Apply<CharismaPower>(new ThrowingPlayerChoiceContext(), Owner.Creature, 2, Owner.Creature, null);
        await PowerCmd.Apply<CombatAdvantagePower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Owner.Creature, 1, this.Owner.Creature, null);
    }
}