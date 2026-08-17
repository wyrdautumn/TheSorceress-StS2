using BaseLib.Abstracts;
using BaseLib.Extensions;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Powers;

namespace TheSorceressMod.TheSorceressModCode.Cards.CrossMod.HeroExpansion;

public class StealthyEnchantment : CustomEnchantmentModel
{
    protected override string? CustomIconPath => "res://TheSorceressMod/images/enchantments/stealthy.png";
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("AddStealthy", 1)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<CombatAdvantagePower>()];
    
    public override bool CanEnchantCardType(CardType cardType) => cardType == CardType.Attack;

    public override bool ShowAmount => true;
    
    public override Decimal EnchantDamageAdditive(Decimal originalDamage, ValueProp props)
    {
        return !props.IsPoweredAttack() || !Card.Owner.HasPower<CombatAdvantagePower>() ? 0M : Amount;
    }
    
    public override bool HasExtraCardText => true;

    protected override void OnEnchant()
    {
        if (!Card.Tags.Contains(SorceressKeywords.Stealthy))
            Card.Tags.AddItem(SorceressKeywords.Stealthy);
        else
            DynamicVars["AddStealthy"].BaseValue = 0;
    }
}