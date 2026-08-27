using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Cards;
using TheSorceressMod.TheSorceressModCode.Powers;

namespace TheSorceressMod.TheSorceressModCode.Cards.Common;

public class TrickStrike() : TheSorceressModCard(1,
    CardType.Attack, CardRarity.Common,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(10,ValueProp.Move)];
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Strike];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<PrimedPower>(), HoverTipFactory.FromPower<CombatAdvantagePower>()];

    protected override bool ShouldGlowGoldInternal 
    {
        get
        {
            if (CombatState != null && CombatState.HittableEnemies.Any(c => c.HasPower<PrimedPower>()))
                return true;
            return Owner.HasPower<CombatAdvantagePower>();
        }
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (play.Target == null)
            return;
        decimal prime = play.Target.GetPowerAmount<PrimedPower>();
        bool hadCA = Owner.HasPower<CombatAdvantagePower>();
        await CommonActions.CardAttack(this, play,vfx:"vfx/vfx_attack_slash").Execute(choiceContext);
        if (prime > 0)
            await CommonActions.Apply<PrimedPower>(choiceContext, play.Target, this, prime);
        if (hadCA)
            await CommonActions.ApplySelf<CombatAdvantagePower>(choiceContext, this, 1);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);
    }
}