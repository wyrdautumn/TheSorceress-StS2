using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using TheSorceressMod.TheSorceressModCode.Powers;

namespace TheSorceressMod.TheSorceressModCode.Cards;

public class CombatAdvantageHelper() : CustomSingletonModel(true,false)
{
    public static readonly SpireField<PlayerCombatState, int> CombatAdvantageCount = new(() => 0);

    public override Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier,
        CardModel? cardSource)
    {
        if (power.Owner.Player == null)
        {
            return base.AfterPowerAmountChanged(choiceContext, power, amount, applier, cardSource);
        }
        PlayerCombatState? playerCombatState = power.Owner.Player.PlayerCombatState;
        if (power is CombatAdvantagePower && amount > 0 && playerCombatState != null)
        {
            int val = CombatAdvantageCount.Get(playerCombatState);
            CombatAdvantageCount.Set(playerCombatState, val +1);
        }
        return base.AfterPowerAmountChanged(choiceContext, power, amount, applier, cardSource);
    }
}