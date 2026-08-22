using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Cards;

namespace TheSorceressMod.TheSorceressModCode.Cards.CrossMod.HeroExpansion;

[Pool(typeof(TokenCardPool))]
public class RendTheVeil() : TheSorceressModHeroExpansionCard(0,
    CardType.Attack, CardRarity.Token,
    TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(21, ValueProp.Move)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust,SorceressKeywords.Shadowdance];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [..AddHeroExpansion()];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (CombatState == null)
            return;
        Color color = new Color("4cba9880");
        double num2 = SaveManager.Instance.PrefsSave.FastMode == FastModeType.Fast ? 0.2 : 0.3;
        NCombatRoom? instance1 = NCombatRoom.Instance;
        if (instance1 != null)
            instance1.CombatVfxContainer.AddChildSafely(NHorizontalLinesVfx.Create(color, 0.8));
        SfxCmd.Play("event:/sfx/characters/ironclad/ironclad_whirlwind");
        NRun? instance2 = NRun.Instance;
        if (instance2 != null)
            instance2.GlobalUi.AddChildSafely(NSmokyVignetteVfx.Create(color, color));
        await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard((CardModel) this, play).TargetingAllOpponents(CombatState).WithHitFx(tmpSfx: "blunt_attack.mp3").WithHitVfxNode((Func<Creature, Node2D>) (t => NGrandFinaleImpactVfx.Create(t) ?? throw new InvalidOperationException())).Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(6);
    }
}