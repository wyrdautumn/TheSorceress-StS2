using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Cards;
using TheSorceressMod.TheSorceressModCode.Cards.CrossMod;
using TheSorceressMod.TheSorceressModCode.Patches;
using TheSorceressMod.TheSorceressModCode.Powers;

namespace TheSorceressMod.TheSorceressModCode.Cards.Ancient;

public class PerfectDistractionAncient : TheSorceressModAncientsAwakenedCard
{
    public PerfectDistractionAncient() : base(0,
        CardType.Skill, CardRarity.Ancient,
        TargetType.AnyEnemy)
    {
        AncientsAwakenedCrossCompat.RegisterExperimentalSerumCardForKalkara(this);
    }
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<PrimedPower>(5)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<PrimedPower>(),HoverTipFactory.FromPower<CombatAdvantagePower>(),HoverTipFactory.Static(SorceressKeywords.Rekindle),HoverTipFactory.Static(SorceressKeywords.AncientsAwakened)];

        public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
        {
            if (player != Owner || this.Pile == null || this.Pile.Type != PileType.Exhaust)
                return;
            await CardPileCmd.Add(this, PileType.Hand);
        }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.Apply<PrimedPower>(choiceContext, this, play);
        await CommonActions.ApplySelf<CombatAdvantagePower>(choiceContext, this, 1);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["PrimedPower"].UpgradeValueBy(2);
    }
}