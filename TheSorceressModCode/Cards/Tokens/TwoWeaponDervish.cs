using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Cards;
using TheSorceressMod.TheSorceressModCode.Powers;

namespace TheSorceressMod.TheSorceressModCode.Cards.Tokens;

[Pool(typeof(TokenCardPool))]
public class TwoWeaponDervish() : TheSorceressModCard(0,
    CardType.Attack, CardRarity.Token,
    TargetType.RandomEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(3, ValueProp.Move), new DynamicVar("mult", 2), new DynamicVar("bonusdamage", 5)];
    
    protected override bool HasEnergyCostX => true;
    
    protected override HashSet<CardTag> CanonicalTags
    {
        get => new HashSet<CardTag>() { SorceressKeywords.TwoWeapon };
    }
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Ethereal,CardKeyword.Exhaust];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (CombatState == null)
            return;
        int num1 = this.ResolveEnergyXValue();
        int hits = num1 * DynamicVars["mult"].IntValue;
        AttackCommand result = await DamageCmd.Attack(DynamicVars.Damage.BaseValue).WithHitCount(hits).FromCard(this).TargetingRandomOpponents(CombatState).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
        List<Creature> enemies = result.Results.SelectMany(r => r.Select(c => c.Receiver)).Distinct().ToList();
        await CreatureCmd.Damage(choiceContext, enemies, DynamicVars["bonusdamage"].BaseValue, ValueProp.Unpowered,
            Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["mult"].UpgradeValueBy(1);
    }
}