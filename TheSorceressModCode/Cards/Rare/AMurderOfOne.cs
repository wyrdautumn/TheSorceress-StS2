using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Cards;
using TheSorceressMod.TheSorceressModCode.Powers;

namespace TheSorceressMod.TheSorceressModCode.Cards.Rare;

public class AMurderOfOne() : TheSorceressModCard(0,
    CardType.Attack, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(4, ValueProp.Move)];
    protected override bool HasEnergyCostX => true;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<CombatAdvantagePower>()];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        int num1 = this.ResolveEnergyXValue();
        if (num1 > 1)
        {
            await PowerCmd.Apply<CombatAdvantagePower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
        }
        if (IsUpgraded)
        {
            num1 += 1;
        }

        await CommonActions.CardAttack(this, play, num1, vfx: "vfx/vfx_attack_slash").Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {

    }
}