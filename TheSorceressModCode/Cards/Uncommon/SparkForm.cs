using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Cards;
using TheSorceressMod.TheSorceressModCode.Powers;

namespace TheSorceressMod.TheSorceressModCode.Cards.Uncommon;

public class SparkForm() : TheSorceressModCard(0,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(6, ValueProp.Move | ValueProp.Unblockable | ValueProp.Unpowered), new PowerVar<WeakPower>(1)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [SorceressKeywords.Sorcery];
    protected override bool HasEnergyCostX => true;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<WeakPower>()];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        SparkForm card = this;
        int num1 = card.ResolveEnergyXValue();
        if (this.CombatState == null)
        {
            return;
        }
        await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
        if (!card.IsUpgraded)
        {
            for (int hitcount = num1; hitcount > 0;)
            {
                --hitcount;
                Creature target = this.CombatState.HittableEnemies.TakeRandom(1, this.Owner.RunState.Rng.CombatTargets)
                    .First();
                VfxCmd.PlayOnCreature(target, "vfx/vfx_attack_lightning");
                SfxCmd.Play("event:/sfx/characters/defect/defect_lightning_passive");
                await CreatureCmd.Damage(choiceContext, target, this.DynamicVars.Damage, (CardModel) this);
                await CommonActions.Apply<WeakPower>(choiceContext, target, this);
            }
        }
        else
        {
            for (int hitcount = num1; hitcount > 0;)
            {
                --hitcount;
                VfxCmd.PlayOnCreatures(this.CombatState.HittableEnemies, "vfx/vfx_attack_lightning");
                SfxCmd.Play("event:/sfx/characters/defect/defect_lightning_evoke");
                await CreatureCmd.Damage(choiceContext, this.CombatState.HittableEnemies, this.DynamicVars.Damage,
                    this.Owner.Creature);
                await PowerCmd.Apply<WeakPower>(choiceContext, this.CombatState.HittableEnemies, this.DynamicVars.Weak.BaseValue,
                    this.Owner.Creature, this);
            }
        }
    }

    protected override void OnUpgrade()
    {

    }
}