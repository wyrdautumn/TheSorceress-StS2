using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Cards;

namespace TheSorceressMod.TheSorceressModCode.Cards.Uncommon;

public class DevourMagic() : TheSorceressModCard(2,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(9, ValueProp.Move | ValueProp.Unblockable | ValueProp.Unpowered)];
    public override bool GainsBlock => true;
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (play.Target != null)
        {
            var damageResults =
                await CreatureCmd.Damage(choiceContext, play.Target, this.DynamicVars.Damage, (CardModel)this);
            var damage = damageResults.Sum(r => r.TotalDamage + r.OverkillDamage);
            await CreatureCmd.GainBlock(this.Owner.Creature, damage, ValueProp.Move, play);

        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);
    }
}