using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Cards;
using TheSorceressMod.TheSorceressModCode.Powers;

namespace TheSorceressMod.TheSorceressModCode.Cards.Uncommon;

public class ShadowSneak() : TheSorceressModCard(0,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override HashSet<CardTag> CanonicalTags => [SorceressKeywords.Stealthy];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(7,ValueProp.Move)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [SorceressKeywords.Sleight];
    

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target,vfx:"vfx/vfx_attack_slash").Execute(choiceContext);
        await Cmd.Wait(0.25f);
    }
    
    protected override (PileType, CardPilePosition) GetResultPileTypeAndPositionForCardPlay()
    {
        (PileType pileType, CardPilePosition cardPilePosition) = base.GetResultPileTypeAndPositionForCardPlay();
        return pileType == PileType.Discard ? (PileType.Hand, CardPilePosition.Bottom) : (pileType, cardPilePosition);
    }
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<CombatAdvantagePower>()];

    protected override void OnUpgrade()
    {
        this.DynamicVars.Damage.UpgradeValueBy(3);
    }
}