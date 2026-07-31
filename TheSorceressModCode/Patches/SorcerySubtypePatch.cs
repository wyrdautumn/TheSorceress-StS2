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
    public static void SorcerySubptypePostfix(NCard __instance)
    {
        if (__instance.Model != null && __instance.Model.Keywords.Contains(SorceressKeywords.Sorcery))
        {
            LocString sorceryString;
            sorceryString = new LocString("gameplay_ui", "THESORCERESSMOD-CARD_TYPE.SORCERY");
            sorceryString.Add("Type",__instance.Model.Type.ToLocString());
            __instance._typeLabel.SetTextAutoSize(sorceryString.GetFormattedText());
            Material sorceryMaterial =
                PreloadManager.Cache.GetMaterial("res://TheSorceressMod/images/shaders/card_banner_sorcery_mat.tres");
            if (__instance._typePlaque.Material != sorceryMaterial)
                __instance._typePlaque.Material = sorceryMaterial;
            Callable.From(new Action(__instance.UpdateTypePlaqueSizeAndPosition)).CallDeferred();
        }
    }
}
