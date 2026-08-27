using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace TheSorceressMod.TheSorceressModCode;

public static class SorceressKeywords
{
    [CustomEnum, KeywordProperties(AutoKeywordPosition.Before)]
    public static CardKeyword Sleight;
    [CustomEnum]
    public static CardKeyword Sorcery;
    [CustomEnum, KeywordProperties(AutoKeywordPosition.After)]
    public static CardKeyword Shadowdance;
    [CustomEnum, KeywordProperties(AutoKeywordPosition.Before)]
    public static CardKeyword Subtle;
    [CustomEnum] public static CardTag TwoWeapon;
    [CustomEnum] public static CardTag Fire;
    [CustomEnum] public static CardTag Stealthy;
    [CustomEnum] public static CardTag Cunning;
    [CustomEnum] public static CardTag PrimeTrick;
    [CustomEnum] public static StaticHoverTip FireAttack;
    [CustomEnum] public static StaticHoverTip Rekindle;
    [CustomEnum] public static StaticHoverTip AncientsAwakened;
    [CustomEnum] public static StaticHoverTip HeroExpansion;
    [CustomEnum] public static StaticHoverTip MoreNeow;
    [CustomEnum] public static VfxDuration ExtraVeryLong;
}