using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Powers;
using TheSorceressMod.TheSorceressModCode.Relics;

namespace TheSorceressMod.TheSorceressModCode.Relics;

public class DancingSash() : TheSorceressModRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Shop;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DancingSashPower>(2)];

    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<StrengthPower>(),HoverTipFactory.FromKeyword(SorceressKeywords.Sleight)];

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner == Owner && cardPlay.Card.Keywords.Contains(SorceressKeywords.Sleight))
        {
            Flash();
            await PowerCmd.Apply<DancingSashPower>(choiceContext, Owner.Creature, DynamicVars["DancingSashPower"].BaseValue, Owner.Creature, null);
        }

    }
}