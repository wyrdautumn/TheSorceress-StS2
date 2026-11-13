using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.RestSite;

namespace TheSorceressMod.TheSorceressModCode.Patches;

[HarmonyPatch]
public class KalkaraRestSitePatch
{
    [HarmonyPatch(typeof(NRestSiteCharacter), nameof(NRestSiteCharacter.FlipX))]
    public class KalkaraRestSiteFlipXPatch
    {
        [HarmonyPostfix]
        private static void Postfix(NRestSiteCharacter __instance)
        {
            AnimatedSprite2D sprite = __instance.GetNodeOrNull<AnimatedSprite2D>("KalkaraSprite");
            if (sprite == null)
                return;
            Control? control = __instance.GetNodeOrNull<Control>("ControlRoot");
            if (control != null)
            {
                sprite.Scale = sprite.Scale with
                {
                    X = -sprite.Scale.X
                };
                sprite.Position = new Vector2(sprite.Position.X + 82, sprite.Position.Y);
                control.Position = new Vector2(control.Position.X + 188, control.Position.Y);
            }
        }
    }

    [HarmonyPatch(typeof(NRestSiteCharacter), nameof(NRestSiteCharacter.HideFlameGlow))]
    public class KalkaraRestSiteHideGlowPatch
    {
        [HarmonyPostfix]
        private static void Postfix(NRestSiteCharacter __instance)
        {
            AnimatedSprite2D sprite = __instance.GetNodeOrNull<AnimatedSprite2D>("KalkaraSprite");
            if (sprite != null)
            {
                sprite.SetAnimation("light_off");
            }
        }
    }
}