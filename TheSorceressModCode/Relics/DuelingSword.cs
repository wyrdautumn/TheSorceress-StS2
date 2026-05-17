using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Helpers.Models;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Runs;
using TheSorceressMod.TheSorceressModCode.helpers;
using TheSorceressMod.TheSorceressModCode.Relics;

namespace TheSorceressMod.TheSorceressModCode.Relics;

public class DuelingSword() : TheSorceressModRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Uncommon;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword(SorceressKeywords.Sorcery)];

    public override Task AfterObtained()
    {
        foreach (CardModel card in PileType.Deck.GetPile(Owner).Cards.Where(c => c.Tags.Contains(CardTag.Strike) || c.Tags.Contains(CardTag.Defend)))
        {
            CardCmd.ApplyKeyword(card, SorceressKeywords.Sorcery);
        }
        return Task.CompletedTask;
    }
    
    public override bool TryModifyCardRewardOptionsLate(
        Player player,
        List<CardCreationResult> cardRewards,
        CardCreationOptions options)
    {
        if (player != this.Owner)
            return false;
        SwordRelicHelper.UpgradeValidCards(cardRewards, (RelicModel) this);
        return true;
    }
    
    public override void ModifyMerchantCardCreationResults(
        Player player,
        List<CardCreationResult> cards)
    {
        if (player != this.Owner)
            return;
        SwordRelicHelper.UpgradeValidCards(cards, (RelicModel) this);
    }
    
    public override bool TryModifyCardBeingAddedToDeck(CardModel card, out CardModel? newCard)
    {
        newCard = (CardModel) null;
        if (card.Owner != this.Owner || (!card.Tags.Contains(CardTag.Strike) && !card.Tags.Contains(CardTag.Defend)))
            return false;
        newCard = this.Owner.RunState.CloneCard(card);
        CardCmd.ApplyKeyword(newCard, SorceressKeywords.Sorcery);
        return true;
    }

    public override Task AfterCardEnteredCombat(CardModel card)
    {
        if (card.Owner == this.Owner && (card.Tags.Contains(CardTag.Strike) || card.Tags.Contains(CardTag.Defend)))
        {
            CardCmd.ApplyKeyword(card, SorceressKeywords.Sorcery);
        }
        return Task.CompletedTask;
    }
    
}