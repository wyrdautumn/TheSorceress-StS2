using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using TheSorceressMod.TheSorceressModCode.Cards.Rare;

namespace TheSorceressMod.TheSorceressModCode.Cards;

public class PrimeTheFireHelper() : CustomSingletonModel(true, false)
{
    public readonly List<CardModel> PrimeTheFirePlayed = new List<CardModel>();

    public override Task BeforeCombatStart()
    {
        PrimeTheFirePlayed.Clear();
        return Task.CompletedTask;
    }

    public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card is PrimeTheFire && !cardPlay.Card.IsDupe)
        {
            PrimeTheFirePlayed.Add(cardPlay.Card);
        }

        return Task.CompletedTask;
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        foreach (CardModel card in PrimeTheFirePlayed.ToList())
        {
            await CardCmd.AutoPlay(choiceContext, card.CreateDupe(), null);
            PrimeTheFirePlayed.Remove(card);
        }
    }

    public override Task AfterCombatEnd(CombatRoom room)
    {
        PrimeTheFirePlayed.Clear();
        return Task.CompletedTask;
    }
}