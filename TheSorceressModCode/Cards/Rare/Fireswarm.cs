using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheSorceressMod.TheSorceressModCode.Cards;
using TheSorceressMod.TheSorceressModCode.Powers;

namespace TheSorceressMod.TheSorceressModCode.Cards.Rare;

public class Fireswarm() : TheSorceressModCard(1,
    CardType.Skill, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<PrimedPower>(2)];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (play.Target == null)
        {
            return;
        }
        await PowerCmd.Apply<PrimedPower>(choiceContext, play.Target, DynamicVars["PrimedPower"].BaseValue,
            Owner.Creature, this);
        await PowerCmd.Apply<FireswarmPower>(choiceContext, play.Target, play.Target.GetPowerAmount<PrimedPower>(),
            Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["PrimedPower"].UpgradeValueBy(2);
    }
}