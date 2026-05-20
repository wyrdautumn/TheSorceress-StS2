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
            if (sprite != null)
            {
                sprite.Scale = sprite.Scale with
                {
                    X = -sprite.Scale.X
                };
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