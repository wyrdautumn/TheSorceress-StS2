using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Patches.Content;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using TheSorceressMod.TheSorceressModCode.helpers;
using TheSorceressMod.TheSorceressModCode.Powers;

namespace TheSorceressMod.TheSorceressModCode.Cards;

public class SleightHelper() : CustomSingletonModel(true, true)
{
    public static readonly SpireField<PlayerCombatState, int> SleightCount = new(() => 0);
    public static readonly SpireField<PlayerCombatState, int> NimbleFingersCount = new(() => 0);

    public override Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        PlayerCombatState? combatState = player.PlayerCombatState;
        if (combatState != null)
        {
            SleightHelper.SleightCount.Set(combatState, 0);
            SleightHelper.NimbleFingersCount.Set(combatState, 0);
        }

        return Task.CompletedTask;
    }

    public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        PlayerCombatState? combatState = cardPlay.Card.Owner.PlayerCombatState;
        if (combatState != null)
        {
            if (cardPlay.Card.Tags.Contains(SorceressKeywords.Cunning))
            {
                SleightCount.Set(combatState, 0);
            }

            if (cardPlay.Card.Keywords.Contains(SorceressKeywords.Sleight))
            {
                if (NimbleFingersCount.Get(combatState) <
                    cardPlay.Card.Owner.Creature.GetPowerAmount<NimbleFingersPower>())
                {
                    int val = NimbleFingersCount.Get(combatState);
                    NimbleFingersCount.Set(combatState, val + 1);
                }
                else
                {
                    int val = SleightHelper.SleightCount.Get(combatState);
                    SleightHelper.SleightCount.Set(combatState, val + 1);
                }
            }

            List<CardModel> playerCards = combatState.AllCards.ToList();
            foreach (CardModel card in playerCards)
            {
                if (card.Keywords.Contains(SorceressKeywords.Sleight) && !card.Owner.HasPower<ConfusedPower>())
                {
                    card.EnergyCost.SetThisTurn(SleightCount.Get(combatState));
                }
            }
        }

        return Task.CompletedTask;
    }

    public override Task AfterCardEnteredCombat(CardModel card)
    {
        if (card.Keywords.Contains(SorceressKeywords.Sleight) && card.Owner.PlayerCombatState != null && !card.Owner.HasPower<ConfusedPower>())
        {
            card.EnergyCost.SetThisTurn(SleightCount.Get(card.Owner.PlayerCombatState));
        }

        return Task.CompletedTask;
    }
}