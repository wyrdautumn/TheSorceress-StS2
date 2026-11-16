using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Cards;

namespace TheSorceressMod.TheSorceressModCode.Cards.Multiplayer;

public class KissOfDeath() : TheSorceressModCard(3,
    CardType.Attack, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
    new CalculationBaseVar(8),
    new ExtraDamageVar(4),
    new CalculatedDamageVar(ValueProp.Move).WithMultiplier(Calc)];
    
    public override CardMultiplayerConstraint MultiplayerConstraint
    {
        get => CardMultiplayerConstraint.MultiplayerOnly;
    }
    
    private static decimal Calc(CardModel card, Creature? target)
    {
        if (target == null)
            return 0;
        return CombatManager.Instance.History.Entries.OfType<PowerReceivedEntry>().Where(p =>
            p.Power.Owner == target && p.Power.Type == PowerType.Debuff && p.Applier != null && p.Applier.IsPlayer).Count();
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (CombatState == null || play.Target == null)
            return;
        Color color = new Color("45276b80");
        double num2 = SaveManager.Instance.PrefsSave.FastMode == FastModeType.Fast ? 0.2 : 0.3;
        NCombatRoom? instance1 = NCombatRoom.Instance;
        if (instance1 != null)
            instance1.CombatVfxContainer.AddChildSafely(NHorizontalLinesVfx.Create(color, 0.8 + num2));
        SfxCmd.Play("event:/sfx/characters/ironclad/ironclad_whirlwind");
        NRun? instance2 = NRun.Instance;
        if (instance2 != null)
            instance2.GlobalUi.AddChildSafely(NSmokyVignetteVfx.Create(color, color));
        await DamageCmd.Attack(DynamicVars.CalculatedDamage).FromCard(this, play).Targeting(play.Target).WithHitFx(vfx: "vfx/vfx_bloody_impact",tmpSfx: "heavy_attack.mp3").Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}