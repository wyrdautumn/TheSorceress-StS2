using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Cards;
using TheSorceressMod.TheSorceressModCode.Cards.Tokens;

namespace TheSorceressMod.TheSorceressModCode.Cards.Multiplayer;

public class TwoWeaponSuppression() : TheSorceressModCard(2,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(12, ValueProp.Move), new PowerVar<WeakPower>(1)];
    protected override HashSet<CardTag> CanonicalTags
    {
        get => new HashSet<CardTag>() { SorceressKeywords.TwoWeapon };
    }
    public override CardMultiplayerConstraint MultiplayerConstraint
    {
        get => CardMultiplayerConstraint.MultiplayerOnly;
    }
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<WeakPower>(),HoverTipFactory.FromCard<TwoWeaponTactics>(IsUpgraded)];
    

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play, 1, vfx: "vfx/vfx_attack_slash").Execute(choiceContext);
        await CommonActions.Apply<WeakPower>(choiceContext, this, play);
        if (CombatState == null)
            return;
        foreach (Creature ally in CombatState.GetTeammatesOf(Owner.Creature)
                     .Where(c => c.IsAlive && c.IsPlayer && c != Owner.Creature))
        {
            if (ally.Player == null)
                continue;
            CardModel tactics = CombatState.CreateCard<TwoWeaponTactics>(ally.Player);
            await CardPileCmd.AddGeneratedCardToCombat(tactics, PileType.Hand, Owner);
            if (IsUpgraded)
                CardCmd.Upgrade(tactics);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4);
        DynamicVars.Weak.UpgradeValueBy(1);
    }
}