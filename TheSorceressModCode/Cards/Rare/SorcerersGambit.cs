using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheSorceressMod.TheSorceressModCode.Cards;
using TheSorceressMod.TheSorceressModCode.Powers;

namespace TheSorceressMod.TheSorceressModCode.Cards.Rare;

public class SorcerersGambit() : TheSorceressModCard(1,
    CardType.Power, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CalculationBaseVar(5),
    new CalculationExtraVar(1),
    new CalculatedVar("SorcerersGambitPower").WithMultiplier(Calc)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [SorceressKeywords.Sorcery];
    
    private static decimal Calc(CardModel card, Creature? arg2)
        => card.Owner.Creature.GetPowerAmount<CharismaPower>();

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<SorcerersGambitPower>(choiceContext, Owner.Creature,
            ((CalculatedVar)DynamicVars["SorcerersGambitPower"]).Calculate(Owner.Creature), Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.CalculationBase.UpgradeValueBy(2);
    }
}