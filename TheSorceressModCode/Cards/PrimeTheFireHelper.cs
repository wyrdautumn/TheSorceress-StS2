using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using TheSorceressMod.TheSorceressModCode.Cards.Rare;

namespace TheSorceressMod.TheSorceressModCode.Cards;

public class PrimeTheFireHelper() : CustomSingletonModel(HookType.Combat)
{
    private readonly List<CardModel> primeTheFirePlayed = new List<CardModel>();

    public override Task BeforeCombatStart()
    {
        primeTheFirePlayed.Clear();
        return Task.CompletedTask;
    }

    public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card is PrimeTheFire && !cardPlay.Card.IsDupe)
        {
            primeTheFirePlayed.Add(cardPlay.Card);
        }

        return Task.CompletedTask;
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        foreach (CardModel card in primeTheFirePlayed.ToList())
        {
            await CardCmd.AutoPlay(choiceContext, card.CreateDupe(), null);
            primeTheFirePlayed.Remove(card);
        }
    }

    public override Task AfterCombatEnd(CombatRoom room)
    {
        primeTheFirePlayed.Clear();
        return Task.CompletedTask;
    }
}