using System.Reflection;
using BaseLib.Utils;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using TheSorceressMod.TheSorceressModCode.Cards.Starter;

namespace TheSorceressMod.TheSorceressModCode;

[ModInitializer(nameof(Initialize))]
public partial class MainFile : Node
{
    public const string ModId = "TheSorceressMod"; //Used for resource filepath
    public const string ResPath = $"res://{ModId}";

    public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } =
        new(ModId, MegaCrit.Sts2.Core.Logging.LogType.Generic);

    public static void Initialize()
    {
        Harmony harmony = new(ModId);

        harmony.PatchAll();
        
        var assembly = Assembly.GetExecutingAssembly();
        Godot.Bridge.ScriptManagerBridge.LookupScriptsInAssembly(assembly);
        
        var deckboxType = AccessTools.TypeByName("MoreNeow.MoreNeowCode.Relics.Complex.UnfamiliarDeckbox");
        if (deckboxType != null)
        {
            var addMethod = AccessTools.DeclaredMethod(deckboxType, "AddCharacterDeck");
            addMethod.Invoke(null, [ModelDb.GetId<Character.TheSorceressMod>(), ModelDb.GetId<TwoWeaponTwist>(), ModelDb.GetId<SparkStep>()]);
        }

    }
    
}