using System.Diagnostics.CodeAnalysis;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Patches.UI;
using BaseLib.Utils;
using BaseLib.Utils.NodeFactories;
using TheSorceressMod.TheSorceressModCode.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.Combat;
using TheSorceressMod.TheSorceressModCode.Cards.Starter;
using TheSorceressMod.TheSorceressModCode.helpers;
using TheSorceressMod.TheSorceressModCode.Relics;

namespace TheSorceressMod.TheSorceressModCode.Character;

[SuppressMessage("Localization", "STS001:Symbol missing localization")]
public class TheSorceressMod : PlaceholderCharacterModel
{
    public const string CharacterId = "TheSorceressMod";

    public static readonly Color Color = new("b18aff");

    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Feminine;
    public override int StartingHp => 70;

    public override IEnumerable<CardModel> StartingDeck =>
    [
        ModelDb.Card<StrikeSorceress>(),
        ModelDb.Card<StrikeSorceress>(),
        ModelDb.Card<StrikeSorceress>(),
        ModelDb.Card<StrikeSorceress>(),
        ModelDb.Card<DefendSorceress>(),
        ModelDb.Card<DefendSorceress>(),
        ModelDb.Card<DefendSorceress>(),
        ModelDb.Card<DefendSorceress>(),
        ModelDb.Card<EnsorcelledBlade>(),
        ModelDb.Card<SkillfulFeint>()
    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<CunningSpark>()
    ];

    public override CardPoolModel CardPool => ModelDb.CardPool<TheSorceressModCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<TheSorceressModRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<TheSorceressModPotionPool>();

    /*  PlaceholderCharacterModel will utilize placeholder basegame assets for most of your character assets until you
        override all the other methods that define those assets.
        These are just some of the simplest assets, given some placeholders to differentiate your character with.
        You don't have to, but you're suggested to rename these images. */
    public override Control CustomIcon
    {
        get
        {
            var icon = NodeFactory<Control>.CreateFromResource(CustomIconTexturePath);
            icon.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            return icon;
        }
    }

    public override string PlaceholderID => "silent";

    public override string CustomIconTexturePath => "character_icon_kalkara.png".CharacterUiPath();
    public override string? CustomIconOutlineTexturePath => "character_icon_kalkara_outline.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "char_select_kalkara.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_kalkara_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_kalkara.png".CharacterUiPath();

    public override string CustomCharacterSelectBg => "res://TheSorceressMod/scenes/char_select_bg_sorceress.tscn";
    public override string CustomVisualPath => "res://TheSorceressMod/scenes/kalkara.tscn";
    public override string CustomEnergyCounterPath => "res://TheSorceressMod/scenes/sorceress_energy_counter.tscn";
    public override string CustomMerchantAnimPath => "res://TheSorceressMod/scenes/sorceress_merchant.tscn";
    public override string CustomRestSiteAnimPath => "res://TheSorceressMod/scenes/sorceress_rest_site.tscn";
    
    public override RelicIconData? CustomYummyCookie => new RelicIconData("res://TheSorceressMod/images/relics/big/yummy_cookie_sorceress.png","res://TheSorceressMod/images/relics/yummy_cookie_sorceress.png","res://TheSorceressMod/images/relics/yummy_cookie_sorceress_outline.png");

    public override Color EnergyLabelOutlineColor => new ("2f3056");
}