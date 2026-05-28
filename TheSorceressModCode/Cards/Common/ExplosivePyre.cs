using System.Collections;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Cards;
using TheSorceressMod.TheSorceressModCode.Powers;

namespace TheSorceressMod.TheSorceressModCode.Cards.Common;

public class ExplosivePyre() : TheSorceressModCard(2,
    CardType.Attack, CardRarity.Common,
    TargetType.AnyEnemy)
{
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(11, ValueProp.Move), new PowerVar<PrimedPower>(4)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [SorceressKeywords.Sorcery];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<PrimedPower>()];
    
    protected override HashSet<CardTag> CanonicalTags
    {
        get => new HashSet<CardTag>() { SorceressKeywords.Fire };
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(this.CombatState);
        await CommonActions.CardAttack(this, play.Target,vfx:"vfx/vfx_attack_slash").BeforeDamage(() =>
            {
                if (play.Target != null)
                {
                    NFireBurningVfx? child = NFireBurningVfx.Create(play.Target,0.8f,true);
                    if (child == null)
                        return Task.CompletedTask;
                    SfxCmd.Play("event:/sfx/characters/attack_fire");
                    NCombatRoom? instance = NCombatRoom.Instance;
                    if (instance != null)
                        instance.CombatVfxContainer.AddChildSafely((Godot.Node)child);
                }
                return Task.CompletedTask;
            }
        ).WithAttackerAnim("Cast",0.2f).Execute(choiceContext);
        await PowerCmd.Apply<PrimedPower>(choiceContext, this.CombatState.HittableEnemies, this.DynamicVars.Power<PrimedPower>().BaseValue, Owner.Creature, this, false);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);
        DynamicVars["PrimedPower"].UpgradeValueBy(2);
    }
}