using BaseLib.Abstracts;
using BaseLib.Patches.Content;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;

namespace TheSorceressMod.TheSorceressModCode.Cards.CrossMod;

public abstract class TheSorceressModAncientsAwakenedCard :
    TheSorceressModCard
{
    protected TheSorceressModAncientsAwakenedCard(int cost, CardType type, CardRarity rarity, TargetType target, bool library = true, bool auto = false) : base(cost, type, rarity, target, library, auto)
    {
        var ancientsAwakenedType =
            AccessTools.TypeByName("AncientsAwakened.AncientsAwakenedCode.Pools.Mithrix.PerfectedPool");
        if (ancientsAwakenedType != null)
        {
            CustomContentDictionary.AddModel(GetType());
        }
    }
}