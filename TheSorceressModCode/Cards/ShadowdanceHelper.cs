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
    
    public override async Task BeforeHandDraw(
        Player player,
        PlayerChoiceContext choiceContext,
        ICombatState combatState)
    {
        List<CardModel> list = PileType.Exhaust.GetPile(player).Cards.ToList();
        foreach (CardModel card in list)
        {
            bool temp = TempShadowdance.Get(card);
            if (temp)
            {
                TempShadowdance.Set(card, false);
                await CardPileCmd.Add(card, PileType.Discard.GetPile(player));
            }
            else if (card.Keywords.Contains(SorceressKeywords.Shadowdance))
            {
                await CardPileCmd.Add(card, PileType.Discard.GetPile(player));
            }
        }
    }

    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool _)
    {
        if (card.CombatState == null)
        {
            return;
        }
        if (card.Keywords.Contains(SorceressKeywords.Subtle) && !WasAgilePlayed.Get(card))
        {
            WasAgilePlayed.Set(card, true);
            await CardCmd.AutoPlay(choiceContext, card,null,AutoPlayType.Default,false,false);
        }

        if (WasAgilePlayed.Get(card))
        {
            WasAgilePlayed.Set(card, false);
        }
    }

    public override (PileType, CardPilePosition) ModifyCardPlayResultPileTypeAndPosition(CardModel card, bool isAutoPlay,
        ResourceInfo resources, PileType pileType, CardPilePosition position)
    {
        if (WasAgilePlayed.Get(card))
        {
            return (PileType.Exhaust, CardPilePosition.Top);
        }
        else
        {
            return (pileType, position);
        }
    }
}