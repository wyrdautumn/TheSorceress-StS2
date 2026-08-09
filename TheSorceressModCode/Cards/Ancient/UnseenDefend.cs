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
using TheSorceressMod.TheSorceressModCode.Patches;
using TheSorceressMod.TheSorceressModCode.Powers;
using TheSorceressMod.TheSorceressModCode.Relics;

namespace TheSorceressMod.TheSorceressModCode.Cards.Ancient;

public class UnseenDefend : TheSorceressModCard
{
    public UnseenDefend() : base(0, CardType.Skill, CardRarity.Token, TargetType.Self)
    {
        AncientsAwakenedCrossCompat.RegisterPerfectedDefendForKalkara(this);
    }
    
    public override CardPoolModel VisualCardPool =>
        AncientsAwakenedCrossCompat.GetPerfectedPoolOrFallback(
            base.VisualCardPool
        );
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(6, ValueProp.Move)];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [SorceressKeywords.Sleight];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<CombatAdvantagePower>()];

    
    public override bool GainsBlock => true;
    
    protected override HashSet<CardTag> CanonicalTags
    {
        get => new HashSet<CardTag>() { CardTag.Defend };
    }

    public override decimal ModifyBlockMultiplicative(Creature target, decimal block, ValueProp props, CardModel? cardSource,
        CardPlay? cardPlay)
    {
        if (target != Owner.Creature || cardSource != this || !props.IsPoweredCardOrMonsterMoveBlock() ||
            !Owner.HasPower<CombatAdvantagePower>())
            return 1M;
        WickedDagger? relic = Owner.GetRelic<WickedDagger>();
        if (relic != null)
        {
            return 1.75M;
        }
        return 1.5M;
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3);
    }
}