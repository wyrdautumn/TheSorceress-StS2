using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheSorceressMod.TheSorceressModCode.Cards;
using TheSorceressMod.TheSorceressModCode.Powers;

namespace TheSorceressMod.TheSorceressModCode.Cards.Rare;

public class Fireswarm() : TheSorceressModCard(0,
    CardType.Skill, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CalculationBaseVar(4),
        new CalculationExtraVar(1),
        new CalculatedVar("Prime").WithMultiplier(Calc)
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<PrimedPower>(),HoverTipFactory.FromPower<CharismaPower>()];
    
    private static decimal Calc(CardModel card, Creature? arg2)
        => card.Owner.Creature.GetPowerAmount<CharismaPower>() / 2;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (play.Target != null)
        {
            await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
            await CommonActions.Apply<PrimedPower>(choiceContext, play.Target, play.Card,
                ((CalculatedVar) DynamicVars["Prime"]).Calculate(Owner.Creature));
        }
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars.CalculationBase.UpgradeValueBy(2);
    }
}