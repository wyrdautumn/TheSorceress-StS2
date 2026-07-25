using HarmonyLib;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;

namespace TheSorceressMod.TheSorceressModCode.Patches;

[HarmonyPatch(typeof(HoverTipFactory), nameof(HoverTipFactory.FromKeyword))]
public class SorceryTooltipIconPatch
{
    [HarmonyPostfix]
    static void AddSorceryIcon(CardKeyword keyword, ref IHoverTip __result)
    {
        if (keyword == SorceressKeywords.Sorcery)
            __result = new HoverTip(new LocString("card_keywords", "THESORCERESSMOD-SORCERY.title"), new LocString("card_keywords", "THESORCERESSMOD-SORCERY.description"), PreloadManager.Cache.GetTexture2D("res://TheSorceressMod/images/charui/sorcery_stamp_text.png"));
    }
}