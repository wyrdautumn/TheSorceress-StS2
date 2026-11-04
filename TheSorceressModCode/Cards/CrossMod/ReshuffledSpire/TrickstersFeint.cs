using BaseLib.Utils;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using TheSorceressMod.TheSorceressModCode.Cards;
using TheSorceressMod.TheSorceressModCode.Cards.Starter;
using TheSorceressMod.TheSorceressModCode.Powers;

namespace TheSorceressMod.TheSorceressModCode.Cards.CrossMod.ReshuffledSpire;

public class TrickstersFeint : TheSorceressModReshuffledSpireCard
{
    public TrickstersFeint() : base(0,
        CardType.Skill, CardRarity.Ancient,
        TargetType.AllEnemies)
    {
        var reshuffledType = AccessTools.TypeByName("ReshuffledSpire.ReshuffledSpireCode.Extensions.AncientVariantExtension");
        if (reshuffledType != null)
        {
            var addMethod = AccessTools.DeclaredMethod(reshuffledType, "RegisterAncientVariant");
            addMethod.Invoke(null,
            [
                ModelDb.GetId<SkillfulFeint>(),ModelDb.GetId<TrickstersFeint>()
            ]);
        }
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<TrickstersFeintDebuff>(4), new CardsVar(1)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [..AddReshuffledSpire(),HoverTipFactory.FromPower<CombatAdvantagePower>(),HoverTipFactory.FromPower<StrengthPower>()];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
        await CommonActions.Draw(this, choiceContext);
        await CommonActions.ApplySelf<CombatAdvantagePower>(choiceContext, this, 1);
        await CommonActions.Apply<TrickstersFeintDebuff>(choiceContext, this, play);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["TrickstersFeintDebuff"].UpgradeValueBy(2);
    }
}