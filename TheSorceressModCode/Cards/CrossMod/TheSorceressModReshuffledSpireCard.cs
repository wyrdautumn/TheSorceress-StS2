using BaseLib.Patches.Content;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace TheSorceressMod.TheSorceressModCode.Cards.CrossMod;

public abstract class TheSorceressModReshuffledSpireCard :
    TheSorceressModCard
{
    public TheSorceressModReshuffledSpireCard(int cost, CardType type, CardRarity rarity, TargetType target,
        bool library = true, bool auto = false) : base(cost, type, rarity, target, library, auto)
    {
        var reshuffledType =
            AccessTools.TypeByName("ReshuffledSpire.ReshuffledSpireCode.Extensions.AncientVariantExtension");
        if (reshuffledType != null)
        {
            CustomContentDictionary.AddModel(GetType());
        }
    }
}