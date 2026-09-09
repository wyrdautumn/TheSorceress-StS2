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

public class DevourMagic() : TheSorceressModCard(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(3, ValueProp.Move | ValueProp.Unblockable | ValueProp.Unpowered), new PowerVar<CharismaPower>(2)];
    public override bool GainsBlock => true;
    public override IEnumerable<CardKeyword> CanonicalKeywords => [SorceressKeywords.Sorcery];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<CharismaPower>()];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (play.Target != null)
        {
            var damageResults =
                await CreatureCmd.Damage(choiceContext, play.Target, this.DynamicVars.Damage, (CardModel)this, play);
            var damage = damageResults.Sum(r => r.TotalDamage + r.OverkillDamage);
            await CreatureCmd.GainBlock(this.Owner.Creature, damage, ValueProp.Move | ValueProp.Unpowered, play);
            await CommonActions.ApplySelf<CharismaPower>(choiceContext, this);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
    }
}