using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheSorceressMod.TheSorceressModCode.Powers;

public class LingeringShadowsPower : TheSorceressModPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];

    public override async Task AfterCardChangedPiles(CardModel card, PileType oldPileType, AbstractModel? source)
    {
        if (card.Owner.Creature != this.Owner || oldPileType != PileType.Exhaust || card.Pile == null || card.Pile.Type == PileType.Exhaust || card.Pile.Type == PileType.None || this.Owner.Player == null || this.Owner.CombatState == null)
        {
            return;
        }
        await Cmd.CustomScaledWait(0.1f, 0.2f);
        Creature? target = this.Owner.Player.RunState.Rng.CombatTargets.NextItem<Creature>((IEnumerable<Creature>) this.Owner.CombatState.HittableEnemies);
        if (target == null)
            return;
        VfxCmd.PlayOnCreatureCenter(target, "vfx/vfx_attack_blunt");
        await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), target, (decimal) this.Amount, ValueProp.Unpowered, this.Owner);
    }
}