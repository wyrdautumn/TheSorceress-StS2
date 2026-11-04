using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheSorceressMod.TheSorceressModCode.Cards;
using TheSorceressMod.TheSorceressModCode.Powers;

namespace TheSorceressMod.TheSorceressModCode.Cards.CrossMod.ReshuffledSpire;

public class ForceOfPersonality() : TheSorceressModReshuffledSpireCard(2,
    CardType.Power, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<ForceOfPersonalityPower>(1), new PowerVar<CharismaPower>(2)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [..AddReshuffledSpire(), HoverTipFactory.FromPower<CharismaPower>()];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
        await CommonActions.ApplySelf<ForceOfPersonalityPower>(choiceContext, this);
        await CommonActions.ApplySelf<CharismaPower>(choiceContext, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["CharismaPower"].UpgradeValueBy(1);
    }
}