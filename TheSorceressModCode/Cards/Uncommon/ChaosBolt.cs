using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Cards;
using TheSorceressMod.TheSorceressModCode.Powers;

namespace TheSorceressMod.TheSorceressModCode.Cards.Uncommon;

public class ChaosBolt() : TheSorceressModCard(2,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(10,ValueProp.Move | ValueProp.Unblockable | ValueProp.Unpowered),
        new CalculationBaseVar(2),
        new CalculationExtraVar(1),
        new CalculatedVar("Debuff").WithMultiplier(Calc)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<WeakPower>(),HoverTipFactory.FromPower<VulnerablePower>(),HoverTipFactory.FromPower<CharismaPower>()];
    
    private static decimal Calc(CardModel card, Creature? arg2)
        => card.Owner.Creature.GetPowerAmount<CharismaPower>() / 2;
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (play.Target == null)
        {
            return;
        }

        int num1 = (int)((CalculatedVar)DynamicVars["Debuff"]).Calculate(Owner.Creature);
        await CreatureCmd.Damage(choiceContext, play.Target, this.DynamicVars.Damage, (CardModel) this);
        for (int loops = num1; loops > 0;)
        {
            --loops;
            if (play.Card.Owner.RunState.Rng.Niche.NextBool())
            {
                await PowerCmd.Apply<WeakPower>(choiceContext, play.Target, 1, this.Owner.Creature, this);
            }
            else
            {
                await PowerCmd.Apply<VulnerablePower>(choiceContext, play.Target, 1, this.Owner.Creature, this);
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
        DynamicVars.CalculationBase.UpgradeValueBy(2);
    }
}