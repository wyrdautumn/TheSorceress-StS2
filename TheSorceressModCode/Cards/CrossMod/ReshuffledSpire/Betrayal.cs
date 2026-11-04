using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Cards;
using TheSorceressMod.TheSorceressModCode.Powers;

namespace TheSorceressMod.TheSorceressModCode.Cards.CrossMod.ReshuffledSpire;

public class Betrayal() : TheSorceressModReshuffledSpireCard(1,
    CardType.Attack, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
    new CalculationBaseVar(14),
    new ExtraDamageVar(4),
    new CalculatedDamageVar(ValueProp.Move).WithMultiplier(Calc)];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [..AddReshuffledSpire(), HoverTipFactory.FromPower<CharismaPower>()];
    
    private static decimal Calc(CardModel card, Creature? arg2)
    {
        return card.Owner.Creature.GetPowerAmount<CharismaPower>() * (card.Owner.Creature.GetPowerAmount<ForceOfPersonalityPower>() + 1);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (play.Target == null)
            return;
        await DamageCmd.Attack(DynamicVars.CalculatedDamage).FromCard((CardModel) this, play).Targeting(play.Target).WithHitFx(tmpSfx: "heavy_attack.mp3").WithHitVfxNode((Func<Creature, Node2D>) (t => (Node2D) NBigSlashVfx.Create(t))).WithHitVfxNode((Func<Creature, Node2D>) (t => (Node2D) NBigSlashImpactVfx.Create(t))).Execute(choiceContext);
        PowerModel? charisma = Owner.Creature.GetPower<CharismaPower>();
        if (charisma != null)
            await PowerCmd.Remove(charisma);
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }
}