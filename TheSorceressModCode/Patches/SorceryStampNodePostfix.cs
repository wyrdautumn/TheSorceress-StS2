using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Cards;
using TheSorceressMod.TheSorceressModCode.helpers;

namespace TheSorceressMod.TheSorceressModCode.Patches;

[HarmonyPatch(typeof(NCard), "_Ready")]
public class SorceryStampNodePostfix
{
    [HarmonyPostfix]
    public static void AddSorceryStamp(NCard __instance)
    {
        var stamp = new NSorceryStamp
        {
            Name = "SorceryStamp",
            MouseFilter = Control.MouseFilterEnum.Ignore
        };

        stamp.SetAnchorsPreset(Control.LayoutPreset.TopLeft);
        stamp.Position = Vector2.Zero;
        stamp.Size = new Vector2(64, 64);
        stamp.ZIndex = 0;

        var visualScene = ResourceLoader.Load<PackedScene>(
            "res://TheSorceressMod/scenes/sorcery_stamp.tscn");

        var visual = visualScene.Instantiate<Control>();
        visual.Name = "SorceryStampVisual";
        visual.MouseFilter = Control.MouseFilterEnum.Ignore;

        stamp.AddChild(visual);
            
        var cardContainer = __instance.GetChild(0)!;
        cardContainer.AddChild(stamp);
    }
}