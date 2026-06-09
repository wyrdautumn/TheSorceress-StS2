using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Cards;
using TheSorceressMod.TheSorceressModCode.Powers;
using TheSorceressMod.TheSorceressModCode.Relics;

namespace TheSorceressMod.TheSorceressModCode.Cards.Uncommon;

public class RhythmBlade() : TheSorceressModCard(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(8, ValueProp.Move)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<CombatAdvantagePower>()];
    public override bool GainsBlock => true;
    
    public override decimal ModifyBlockMultiplicative(Creature target, decimal block, ValueProp props, CardModel? cardSource,
        CardPlay? cardPlay)
    {
        if (cardSource == this && this.Owner.HasPower<CombatAdvantagePower>())
        {
            return 2;
        }
        return base.ModifyBlockMultiplicative(target, block, props, cardSource, cardPlay);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(2);
    }
}