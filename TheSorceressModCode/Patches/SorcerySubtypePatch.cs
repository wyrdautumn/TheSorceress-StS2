using System.Reflection.Emit;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;

namespace TheSorceressMod.TheSorceressModCode.Patches;

[HarmonyPatch(typeof(NCard), "UpdateTypePlaque")]
public class SorcerySubtypePatch
{
    [HarmonyPostfix]
    public static void SorcerySubtypePostfix(NCard __instance)
    {
        CardModel? card = __instance.Model;
        if (card != null && (card.Keywords.Contains(SorceressKeywords.Sorcery) || card.Tags.Contains(SorceressKeywords.TwoWeapon)))
        {
            LocString sorceryString;
            if (card.Keywords.Contains(SorceressKeywords.Sorcery) && card.Tags.Contains(SorceressKeywords.TwoWeapon))
                sorceryString = new LocString("gameplay_ui", "THESORCERESSMOD-CARD_TYPE.TWO_WEAPON_SORCERY");
            else if (card.Keywords.Contains(SorceressKeywords.Sorcery))
                sorceryString = new LocString("gameplay_ui", "THESORCERESSMOD-CARD_TYPE.SORCERY");
            else
                sorceryString = new LocString("gameplay_ui", "THESORCERESSMOD-CARD_TYPE.TWO_WEAPON");
            sorceryString.Add("Type",card.Type.ToLocString());
            __instance._typeLabel.SetTextAutoSize(sorceryString.GetFormattedText());
            if (card.Keywords.Contains(SorceressKeywords.Sorcery))
            {
                Material sorceryMaterial =
                    PreloadManager.Cache.GetMaterial(
                        "res://TheSorceressMod/images/shaders/card_banner_sorcery_mat.tres");
                if (__instance._typePlaque.Material != sorceryMaterial)
                    __instance._typePlaque.Material = sorceryMaterial;
            }
            Callable.From(new Action(__instance.UpdateTypePlaqueSizeAndPosition)).CallDeferred();
        }
    }
}
