using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Relics;

namespace TheSorceressMod.TheSorceressModCode.Relics;

public class DancingSash() : TheSorceressModRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Rare;
    
    

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<StrengthPower>(),HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];

    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
    {
        if (card.Owner == Owner)
        {
            Flash();
            await PowerCmd.Apply<TemporaryStrengthPower>(choiceContext, Owner.Creature, 1, Owner.Creature, null);
        }
    }

    public override async Task AfterCardDiscarded(PlayerChoiceContext choiceContext, CardModel card)
    {
        if (card.Owner == Owner)
        {
            Flash();
            await PowerCmd.Apply<TemporaryStrengthPower>(choiceContext, Owner.Creature, 1, Owner.Creature, null);
        }
    }
}