using BaseLib.Patches.Content;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace TheSorceressMod.TheSorceressModCode.Cards.CrossMod;

public abstract class TheSorceressModMoreNeowCard :
    TheSorceressModCard
{
    public TheSorceressModMoreNeowCard(int cost, CardType type, CardRarity rarity, TargetType target, bool library = true, bool auto = false) : base(cost, type, rarity, target, library, auto)
    {
        var deckboxType = AccessTools.TypeByName("MoreNeow.MoreNeowCode.Relics.Complex.UnfamiliarDeckbox");
        if (deckboxType != null)
        {
            CustomContentDictionary.AddModel(GetType());
        }
    }
}