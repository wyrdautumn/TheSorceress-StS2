using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Cards;
using TheSorceressMod.TheSorceressModCode.Powers;

namespace TheSorceressMod.TheSorceressModCode.Cards.Uncommon;

public class AThousandMasks() : TheSorceressModCard(3,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(4, ValueProp.Move), new DynamicVar("AdvantageCount",0)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<CombatAdvantagePower>()];
    
    public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (this.Owner.PlayerCombatState != null)
        {
            this.DynamicVars["AdvantageCount"].BaseValue =
                CombatAdvantageHelper.CombatAdvantageCount.Get(this.Owner.PlayerCombatState);
        }

        return base.AfterCardPlayed(choiceContext, cardPlay);
    }

    public override Task AfterCardEnteredCombat(CardModel card)
    {
        if (card == this && this.Owner.PlayerCombatState != null)
        {
            this.DynamicVars["AdvantageCount"].BaseValue =
                CombatAdvantageHelper.CombatAdvantageCount.Get(this.Owner.PlayerCombatState);
        }
        return base.AfterCardEnteredCombat(card);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play, DynamicVars["AdvantageCount"].IntValue).Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}