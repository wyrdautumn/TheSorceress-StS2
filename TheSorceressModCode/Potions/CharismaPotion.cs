using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using TheSorceressMod.TheSorceressModCode.Powers;

namespace TheSorceressMod.TheSorceressModCode.Potions;

public class CharismaPotion : TheSorceressModPotion
{
    public override PotionRarity Rarity => PotionRarity.Uncommon;

    public override PotionUsage Usage => PotionUsage.CombatOnly;

    public override TargetType TargetType => TargetType.Self;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<CharismaPower>(2)];
    
    public override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<CharismaPower>()];

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        NCombatRoom.Instance?.PlaySplashVfx(Owner.Creature, new Godot.Color("fab8c6"));
        await PowerCmd.Apply<CharismaPower>(choiceContext, Owner.Creature, DynamicVars["CharismaPower"].BaseValue, Owner.Creature, null);
    }
    
    public override string? CustomPackedImagePath => "res://TheSorceressMod/images/potions/charisma_potion.png";
    public override string? CustomPackedOutlinePath => "res://TheSorceressMod/images/potions/charisma_potion_outline.png";
}