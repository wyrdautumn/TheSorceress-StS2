using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using TheSorceressMod.TheSorceressModCode.Powers;

namespace TheSorceressMod.TheSorceressModCode.Potions;

public class ShadowPotion : TheSorceressModPotion
{
    public override PotionRarity Rarity => PotionRarity.Rare;

    public override PotionUsage Usage => PotionUsage.CombatOnly;

    public override TargetType TargetType => TargetType.Self;
    
    public override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        NCombatRoom.Instance?.PlaySplashVfx(Owner.Creature, new Godot.Color("695985"));
        var prefs = new CardSelectorPrefs(new LocString("card_selection", "TO_SHADOW"), 1);
        var card = (await CardSelectCmd.FromSimpleGrid(
            choiceContext,
            PileType.Exhaust.GetPile(Owner).Cards,
            Owner,
            prefs)).FirstOrDefault();
        if (card == null)
        {
            return;
        }
        await CardCmd.AutoPlay(choiceContext, card, null);
    }
    
    public override string? CustomPackedImagePath => "res://TheSorceressMod/images/potions/shadow_potion.png";
    public override string? CustomPackedOutlinePath => "res://TheSorceressMod/images/potions/shadow_potion_outline.png";
}