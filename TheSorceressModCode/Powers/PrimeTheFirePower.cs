using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Cards.Rare;

namespace TheSorceressMod.TheSorceressModCode.Powers;

public class PrimeTheFirePower : TheSorceressModPower, IHasSecondAmount
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [..HoverTipFactory.FromCardWithCardHoverTips<PrimeTheFire>()];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<PrimedPower>(0)];
    
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != this.Owner.Player)
            return;
        this.Flash();
        await CreatureCmd.Damage(choiceContext, CombatState.HittableEnemies, Amount, ValueProp.Unpowered, Owner, null);
        await PowerCmd.Apply<PrimedPower>(choiceContext, CombatState.HittableEnemies, DynamicVars["PrimedPower"].BaseValue, Owner, null);
        await PowerCmd.Remove(this);
    }

    public string GetSecondAmount()
    {
        return DynamicVars["PrimedPower"].IntValue.ToString();
    }
}