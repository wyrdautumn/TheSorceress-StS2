using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace TheSorceressMod.TheSorceressModCode;

public static class SorceressKeywords
{
    [CustomEnum, KeywordProperties(AutoKeywordPosition.Before)]
    public static CardKeyword Sleight;
    [CustomEnum, KeywordProperties(AutoKeywordPosition.Before)]
    public static CardKeyword Sorcery;
    [CustomEnum, KeywordProperties(AutoKeywordPosition.After)]
    public static CardKeyword Shadowdance;
    [CustomEnum, KeywordProperties(AutoKeywordPosition.Before)]
    public static CardKeyword Subtle;
    [CustomEnum] public static CardTag TwoWeapon;
    [CustomEnum] public static CardTag Fire;
    [CustomEnum] public static CardTag Stealthy;
    [CustomEnum] public static CardTag Cunning;
}