using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Cards;
using TheSorceressMod.TheSorceressModCode.Powers;

namespace TheSorceressMod.TheSorceressModCode.Cards.Rare;

public class Translocate() : TheSorceressModCard(1,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(8, ValueProp.Move),
    new CalculationBaseVar(2),
    new CalculationExtraVar(1),
    new CalculatedVar("CardDraw").WithMultiplier(Calc)];
    public override bool GainsBlock => true;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<CharismaPower>()];
    
    private static decimal Calc(CardModel card, Creature? arg2)
        => card.Owner.Creature.GetPowerAmount<CharismaPower>() / 3;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
        IEnumerable<CardModel> cards = (IEnumerable<CardModel>) PileType.Hand.GetPile(this.Owner).Cards;
        await CardCmd.DiscardAndDraw(choiceContext, cards, (int) ((CalculatedVar) DynamicVars["CardDraw"]).Calculate(Owner.Creature));
    }

    protected override void OnUpgrade()
    {
        DynamicVars.CalculationBase.UpgradeValueBy(1);
    }
}