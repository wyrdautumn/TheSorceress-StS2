using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Helpers.Models;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Rooms;
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

    public override Task AfterCardEnteredCombat(CardModel card)
    {
        if (!CanAffect(card))
            return Task.CompletedTask;
        CardCmd.ApplyKeyword(card, SorceressKeywords.Sorcery);
        return Task.CompletedTask;
    }

    public override Task AfterRoomEntered(AbstractRoom room)
    {
        if (!(room is CombatRoom) || this.Owner.PlayerCombatState == null)
            return Task.CompletedTask;
        foreach (CardModel allCard in this.Owner.PlayerCombatState.AllCards)
        {
            if (CanAffect(allCard))
                CardCmd.ApplyKeyword(allCard, SorceressKeywords.Sorcery);
        }
        return Task.CompletedTask;
    }

    private static bool CanAffect(CardModel card)
    {
        return card.Rarity == CardRarity.Basic && (card.Tags.Contains<CardTag>(CardTag.Strike) || card.Tags.Contains<CardTag>(CardTag.Defend)) && !card.GetKeywordsWithSources(KeywordSources.Local).Contains(CardKeyword.Ethereal);
    }
    
}