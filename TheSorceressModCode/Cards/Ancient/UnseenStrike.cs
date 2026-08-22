using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Cards;
using TheSorceressMod.TheSorceressModCode.Cards.CrossMod;
using TheSorceressMod.TheSorceressModCode.Patches;
using TheSorceressMod.TheSorceressModCode.Powers;

namespace TheSorceressMod.TheSorceressModCode.Cards.Ancient;

public class UnseenStrike : TheSorceressModAncientsAwakenedCard
{
    public UnseenStrike() : base(1, CardType.Attack, CardRarity.Token, TargetType.AnyEnemy)
    {
        AncientsAwakenedCrossCompat.RegisterPerfectedStrikeForKalkara(this);
    }
    
    public override CardPoolModel VisualCardPool =>
        AncientsAwakenedCrossCompat.GetPerfectedPoolOrFallback(
            base.VisualCardPool
        );

    protected override HashSet<CardTag> CanonicalTags =>
    [
        CardTag.Strike, SorceressKeywords.Stealthy, SorceressKeywords.PrimeTrick
    ];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<CombatAdvantagePower>(),..AddAncientsAwakened()];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [SorceressKeywords.Subtle];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(7, ValueProp.Move)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play,vfx:"vfx/vfx_attack_slash").Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);
    }
}