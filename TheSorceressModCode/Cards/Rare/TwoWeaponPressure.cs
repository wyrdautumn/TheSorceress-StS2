using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Cards;
using TheSorceressMod.TheSorceressModCode.Cards.Tokens;

#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.

namespace TheSorceressMod.TheSorceressModCode.Cards.Rare;

public class TwoWeaponPressure() : TheSorceressModCard(2,
    CardType.Attack, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(10, ValueProp.Move), new PowerVar<WeakPower>(1)];
    protected override HashSet<CardTag> CanonicalTags
    {
        get => new HashSet<CardTag>() { SorceressKeywords.TwoWeapon };
    }

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<WeakPower>(), HoverTipFactory.FromCard<TwoWeaponOpening>(false)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (play.Target == null)
            return;
        await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard((CardModel) this).Targeting(play.Target).WithHitFx(tmpSfx: "heavy_attack.mp3").WithHitVfxNode((Func<Creature, Node2D>) (t => (Node2D) NBigSlashVfx.Create(t))).WithHitVfxNode((Func<Creature, Node2D>) (t => (Node2D) NBigSlashImpactVfx.Create(t))).Execute(choiceContext);
        await CommonActions.Apply<WeakPower>(choiceContext, play.Target, this);
        if (CombatState == null)
            return;
        CardModel open = CombatState.CreateCard<TwoWeaponOpening>(Owner);
        CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(open, PileType.Draw, Owner,CardPilePosition.Random), 1.5F);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4);
        DynamicVars.Weak.UpgradeValueBy(1);
    }
}