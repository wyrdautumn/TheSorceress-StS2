using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Cards;
using TheSorceressMod.TheSorceressModCode.Cards.Tokens;

namespace TheSorceressMod.TheSorceressModCode.Cards.CrossMod.ReshuffledSpire;

public class CrossCut() : TheSorceressModReshuffledSpireCard(1,
    CardType.Attack, CardRarity.Common,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(6, ValueProp.Move), new PowerVar<WeakPower>(1)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [..AddReshuffledSpire(),HoverTipFactory.FromPower<WeakPower>(),HoverTipFactory.FromCard<CrossCrush>(IsUpgraded),HoverTipFactory.FromPower<VulnerablePower>()];
    
    protected override HashSet<CardTag> CanonicalTags
    {
        get => new HashSet<CardTag>() { SorceressKeywords.TwoWeapon };
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play, vfx: "vfx/vfx_attack_slash").Execute(choiceContext);
        if (play.Target != null)
            await CommonActions.Apply<WeakPower>(choiceContext, play.Target, this);
        if (CombatState == null)
            return;
        CardModel crush = CombatState.CreateCard<CrossCrush>(Owner);
        await CardPileCmd.AddGeneratedCardToCombat(crush, PileType.Hand, Owner);
        if (IsUpgraded)
            CardCmd.Upgrade(crush);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);
    }
}