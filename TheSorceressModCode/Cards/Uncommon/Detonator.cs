using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Cards;
using TheSorceressMod.TheSorceressModCode.Powers;

namespace TheSorceressMod.TheSorceressModCode.Cards.Uncommon;

public class Detonator() : TheSorceressModCard(2,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(14, ValueProp.Move), new PowerVar<DetonatorPower>(1)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword(SorceressKeywords.Subtle), HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (play.Target == null)
            return;
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this, play).Targeting(play.Target).WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3")
            .Execute(choiceContext);
        await CommonActions.ApplySelf<DetonatorPower>(choiceContext, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(6);
    }
}