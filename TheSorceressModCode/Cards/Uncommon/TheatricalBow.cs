using BaseLib.Utils;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using TheSorceressMod.TheSorceressModCode.Cards;

namespace TheSorceressMod.TheSorceressModCode.Cards.Uncommon;

public class TheatricalBow() : TheSorceressModCard(0,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CalculationBaseVar(0),
        new CalculationExtraVar(1),
        new CalculatedVar("Skills").WithMultiplier(Calc)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<VigorPower>()];
    
    private static decimal Calc(CardModel card, Creature? arg2)
        => (Decimal)CombatManager.Instance.History.CardPlaysFinished.Count<CardPlayFinishedEntry>(
            (Func<CardPlayFinishedEntry, bool>)(e =>
                e.HappenedThisTurn(card.CombatState) && e.CardPlay.Card.Type == CardType.Skill &&
                e.CardPlay.Card.Owner == card.Owner));

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
        await CommonActions.ApplySelf<VigorPower>(choiceContext,this, ((CalculatedVar)DynamicVars["Skills"]).Calculate(Owner.Creature));
    }

    protected override void OnUpgrade()
    {
        AddKeyword(SorceressKeywords.Shadowdance);
    }
}