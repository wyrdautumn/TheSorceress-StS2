using BaseLib.Patches.Content;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace TheSorceressMod.TheSorceressModCode.Cards.CrossMod;

public abstract class TheSorceressModHeroExpansionCard :
    TheSorceressModCard
{
    public TheSorceressModHeroExpansionCard(int cost, CardType type, CardRarity rarity, TargetType target, bool library = true, bool auto = false) : base(cost, type, rarity, target, library, auto)
    {
        var mysteriousFlashlightType =
            AccessTools.TypeByName("TheHeroExpansion.TheHeroExpansionCode.Extensions.CustomMysteriousFlashlightExtension");
        if (mysteriousFlashlightType != null)
            CustomContentDictionary.AddModel(GetType());
    }
}