using System.Runtime.ExceptionServices;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheSorceressMod.TheSorceressModCode.Cards;

namespace TheSorceressMod.TheSorceressModCode.Cards.Uncommon;

public class ArcingFire() : TheSorceressModCard(2,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(15, ValueProp.Move)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [SorceressKeywords.Sorcery];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArcingFire cardSource = this;
        if (cardSource.CombatState == null || play.Target == null)
        {
            return;
        }
        AttackCommand attackCommand = await DamageCmd.Attack(cardSource.DynamicVars.Damage.BaseValue).FromCard((CardModel) cardSource).Targeting(play.Target).WithHitFx(vfx:"vfx/vfx_fire_burst",sfx:"event:/sfx/characters/attack_fire").WithAttackerAnim("Cast",0.2f).Execute(choiceContext);
        if (attackCommand.Results
            .SelectMany<List<DamageResult>, DamageResult>(
                (Func<List<DamageResult>, IEnumerable<DamageResult>>)(r => (IEnumerable<DamageResult>)r))
            .Any<DamageResult>((Func<DamageResult, bool>)(r => r.WasTargetKilled)))
        {
            CardModel dupe = cardSource.CreateDupe();
            await CardCmd.AutoPlay(choiceContext, dupe, null);
        }
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars.Damage.UpgradeValueBy(5);
    }
}