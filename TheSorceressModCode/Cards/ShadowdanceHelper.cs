using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Rngs;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using TheSorceressMod.TheSorceressModCode.Relics;

namespace TheSorceressMod.TheSorceressModCode.Cards;

public class ShadowdanceHelper() : CustomSingletonModel(HookType.Combat)
{
    public static readonly SpireField<CardModel, bool> TempShadowdance = new(() => false);
    public static readonly SpireField<CardModel, bool> WasAgilePlayed = new(() => false);
    public static readonly SpireField<CardModel, bool> ExhaustedOnPlay = new(() => false);
    public static readonly SpireField<PlayerCombatState, int> CardsDanced = new(() => 0);
    
    public override async Task AfterCardChangedPiles(CardModel card, PileType oldPileType, AbstractModel? source)
    {
        if (oldPileType == PileType.Exhaust && card.Pile != null && card.Pile.Type != PileType.Exhaust && card.Pile.Type != PileType.None && card.Pile.Type != PileType.Play && card.Owner.PlayerCombatState != null)
        {
            int val = CardsDanced.Get(card.Owner.PlayerCombatState);
            CardsDanced.Set(card.Owner.PlayerCombatState, val + 1);
        }
    }
    
    public override async Task BeforeHandDrawLate(
        Player player,
        PlayerChoiceContext choiceContext,
        ICombatState combatState)
    {
        if (player.PlayerCombatState == null)
            return;
        List<CardModel> list = player.PlayerCombatState.AllCards.ToList();
        foreach (CardModel card in list)
        {
            if (TempShadowdance.Get(card) || card.Keywords.Contains(SorceressKeywords.Shadowdance) && card.Pile != null && card.Pile.Type == PileType.Exhaust)
            {
                await CardPileCmd.Add(card, PileType.Discard.GetPile(player));
            }
            TempShadowdance.Set(card, false);
        }
    }

    public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.ResultPile == PileType.Exhaust)
            ExhaustedOnPlay.Set(cardPlay.Card, true);
        return Task.CompletedTask;
    }

    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool _)
    {
        if (card.CombatState == null)
        {
            return;
        }

        if (ExhaustedOnPlay.Get(card))
        {
            ExhaustedOnPlay.Set(card, false);
            return;
        }
        if (card.Keywords.Contains(SorceressKeywords.Subtle))
        {
            WasAgilePlayed.Set(card, true);
            await CardCmd.AutoPlay(choiceContext, card,null,AutoPlayType.Default,false,false);
            WasAgilePlayed.Set(card, false);
        }
    }

    public override CardLocation ModifyCardPlayResultLocation(CardModel card, bool isAutoPlay, ResourceInfo resources,
        CardLocation cardLocation)
    {
        if (WasAgilePlayed.Get(card))
        {
            cardLocation.pileType = PileType.Exhaust;
            return cardLocation;
        }
        else
        {
            return cardLocation;
        }
    }
}