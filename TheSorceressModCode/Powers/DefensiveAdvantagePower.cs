using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TheSorceressMod.TheSorceressModCode.Powers;

public class DefensiveAdvantagePower : TheSorceressModPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<CombatAdvantagePower>(),HoverTipFactory.FromPower<StrengthPower>()];

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (Owner.CombatState != null && side == CombatSide.Player && this.Owner.HasPower<CombatAdvantagePower>())
        {
            this.Flash();
            foreach (Creature enemy in Owner.CombatState.HittableEnemies)
            {
                await PowerCmd.Apply<DefensiveAdvantageDebuff>(choiceContext, enemy, Amount, Owner, null);
            }
        }
    }
}