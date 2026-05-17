using BaseLib.Patches.Features;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Cards;

namespace TheSorceressMod.TheSorceressModCode.Cards.Uncommon;

public class TwoWeaponRiposte() : TheSorceressModCard(1,
    CardType.Attack, CardRarity.Uncommon,
    CustomTargetType.AnyAttackingEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(7, ValueProp.Move), new BlockVar(7, ValueProp.Move)];

    public override bool GainsBlock => true;
    
    protected override HashSet<CardTag> CanonicalTags
    {
        get => new HashSet<CardTag>() { SorceressKeywords.TwoWeapon };
    }

    protected override bool IsPlayable
    {
        get
        {
            if (this.CombatState == null)
            {
                return false;
            }
            
            foreach (Creature enemy in this.CombatState.Enemies)
            {
                if (enemy.Monster != null && enemy.Monster.IntendsToAttack)
                {
                    return true;
                }
            }
            
            return false;
        }
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
        await CommonActions.CardAttack(this, play, vfx: "vfx/vfx_attack_slash").Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
        DynamicVars.Block.UpgradeValueBy(2);
    }
}