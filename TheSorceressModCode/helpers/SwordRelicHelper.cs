using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

namespace TheSorceressMod.TheSorceressModCode.helpers;

public class SwordRelicHelper
{
    public static void UpgradeValidCards(
        List<CardCreationResult> cards,
        RelicModel swordRelic)
    {
        foreach (CardCreationResult card1 in cards)
        {
            CardModel card2 = card1.Card;
            if (card2.Tags.Contains(CardTag.Defend) || card2.Tags.Contains(CardTag.Strike))
            {
                CardModel card3 = swordRelic.Owner.RunState.CloneCard(card2);
                CardCmd.ApplyKeyword(card3,SorceressKeywords.Sorcery);
                card1.ModifyCard(card3, swordRelic);
            }
        }
    }
}