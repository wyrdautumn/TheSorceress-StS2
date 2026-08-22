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

namespace TheSorceressMod.TheSorceressModCode.Cards.CrossMod.HeroExpansion;

public class SpellRiposte() : TheSorceressModHeroExpansionCard(0,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(4, ValueProp.Move), new PowerVar<PrimedPower>(8)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [SorceressKeywords.Sleight];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<PrimedPower>(),..AddHeroExpansion()];
    
    protected override bool ShouldGlowGoldInternal
    {
        get
        {
            if (CombatState == null)
            {
                return false;
            }
            return CombatState.HittableEnemies.Any(e => e.Monster != null && !e.Monster.IntendsToAttack);
        }
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play, vfx: "vfx/vfx_starry_impact", sfx: "blunt_attack.mp3").WithAttackerAnim("Cast",0.2f).Execute(choiceContext);
        if (play.Target != null && play.Target.Monster != null && !play.Target.Monster.IntendsToAttack)
            await CommonActions.Apply<PrimedPower>(choiceContext, this, play);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["PrimedPower"].UpgradeValueBy(4);
    }
}