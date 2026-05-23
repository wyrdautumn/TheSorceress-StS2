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
using MegaCrit.Sts2.Core.Runs;
using TheSorceressMod.TheSorceressModCode.Relics;

namespace TheSorceressMod.TheSorceressModCode.Cards;

public class ShadowdanceHelper() : CustomSingletonModel(HookType.Combat)
{
    public static readonly SpireField<CardModel, bool> TempShadowdance = new(() => false);

    public override async Task AfterAutoPrePlayPhaseEnteredEarly(PlayerChoiceContext choiceContext, Player player)
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
                Voidheart? relic = player.GetRelic<Voidheart>();
                if (relic != null)
                {
                    await CardPileCmd.Add(card, PileType.Hand.GetPile(player));
                }
                else
                {
                    await CardPileCmd.Add(card, PileType.Discard.GetPile(player));
                }
            }
        }
    }

    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool _)
    {
        if (card.CombatState == null)
        {
            return;
        }
        if (card.Keywords.Contains(SorceressKeywords.Subtle))
        {
            CardModel card2 = card.CreateDupe();
            await CardCmd.AutoPlay(choiceContext, card2,null,AutoPlayType.Default,false,false);
        }
    }
}