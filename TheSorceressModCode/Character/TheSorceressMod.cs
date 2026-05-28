using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
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
using MegaCrit.Sts2.Core.Nodes.Vfx;
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
    public override string CustomTrailPath => "res://TheSorceressMod/scenes/card_trail_sorceress.tscn";

    public override string CustomCharacterSelectTransitionPath =>
        "res://TheSorceressMod/images/shaders/sorceress_transition_mat.tres";

    public override string CharacterTransitionSfx => "res://TheSorceressMod/audio/sorceress_transition.wav";
    public override string CustomArmPointingTexturePath => "res://TheSorceressMod/images/hands/sorceress_hand_point.png";
    public override string CustomArmRockTexturePath => "res://TheSorceressMod/images/hands/sorceress_hand_rock.png";
    public override string CustomArmPaperTexturePath => "res://TheSorceressMod/images/hands/sorceress_hand_paper.png";
    public override string CustomArmScissorsTexturePath => "res://TheSorceressMod/images/hands/sorceress_hand_scissors.png";
    public override RelicIconData? CustomYummyCookie => new RelicIconData("res://TheSorceressMod/images/relics/big/yummy_cookie_sorceress.png","res://TheSorceressMod/images/relics/yummy_cookie_sorceress.png","res://TheSorceressMod/images/relics/yummy_cookie_sorceress_outline.png");

    public override Color EnergyLabelOutlineColor => new ("2f3056");
    public override Color DialogueColor => new ("4D3970");
    public override VfxColor SpeechBubbleColor => VfxColor.Purple;
    public override Color MapDrawingColor => new ("7B4EC8");
    public override Color RemoteTargetingLineColor => new ("A17CEAFF");
    public override Color RemoteTargetingLineOutline => new ("714CBCFF");
    
    public override List<string> GetArchitectAttackVfx()
    {
        int num = 5;
        List<string> list = new List<string>(num);
        CollectionsMarshal.SetCount<string>(list, num);
        Span<string> span = CollectionsMarshal.AsSpan<string>(list);
        int index1 = 0;
        span[index1] = "vfx/vfx_attack_slash";
        int index2 = index1 + 1;
        span[index2] = "vfx/vfx_attack_blunt";
        int index3 = index2 + 1;
        span[index3] = "vfx/vfx_bloody_impact";
        int index4 = index3 + 1;
        span[index4] = "vfx/vfx_fire_burst";
        int index5 = index4 + 1;
        span[index5] = "vfx/vfx_rock_shatter";
        return list;
    }
}