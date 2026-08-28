using BaseLib.Extensions;
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
using TheSorceressMod.TheSorceressModCode.Relics;

namespace TheSorceressMod.TheSorceressModCode.Cards.Uncommon;

public class AMurderOfOne() : TheSorceressModCard(3,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
    new CalculationBaseVar(18),
    new ExtraDamageVar(4),
    new CalculatedDamageVar(ValueProp.Move).WithMultiplier(Calc)
    ];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<CombatAdvantagePower>()];

    private static decimal Calc(CardModel card, Creature? arg2)
    {
        if (card.Owner.PlayerCombatState == null)
            return 0;
        return ShadowdanceHelper.CardsDanced.Get(card.Owner.PlayerCombatState);
    }

    public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer,
        CardModel? cardSource, CardPlay? cardPlay)
    {
        if (!Owner.HasPower<CombatAdvantagePower>() && cardSource == this)
        {
            WickedDagger? relic = Owner.GetRelic<WickedDagger>();
            if (relic != null)
            {
                return 1.75M;
            }
            return 1.5M;
        }
        return 1M;
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<CombatAdvantagePower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
        if (play.Target != null)
            VfxCmd.PlayOnCreatureCenter(play.Target, "vfx/vfx_starry_impact");
        await CommonActions.CardAttack(this, play, vfx: "vfx/vfx_dramatic_stab").Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(1);
    }
}